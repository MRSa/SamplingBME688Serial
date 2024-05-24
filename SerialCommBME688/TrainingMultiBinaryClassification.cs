using Microsoft.ML;
using System.Diagnostics;

namespace SamplingBME688Serial
{
    public enum MultiClassMethod
    {
        PairwiseCoupling,
        OneVersusAll
    }

    public enum BinaryClassificationMethod
    {
        LightGbm,
        Gam,
        FastTree,
        FastForest,
        AveragedPerceptron,
        LbfgsLogisticRegression,
        SymbolicSgdLogisticRegression
    }

    class TrainingMultiBinaryClassification : ICreatePredictionModel, IPredictionModel
    {
        private MLContext mlContext;
        private string inputDataFileName;
        private string classificationMethod;
        private MultiClassMethod multiClassMethod;
        private BinaryClassificationMethod binaryClassificationMethod;
        private IEstimator<ITransformer>? estimator = null;
        private ICreateModelConsole console;
        private ITransformer? trainedModel = null;
        private IDataView? dataView = null;

        public TrainingMultiBinaryClassification(ref MLContext mlContext, MultiClassMethod method, BinaryClassificationMethod binaryClassification, string inputDataFileName, ICreateModelConsole console)
        {
            this.mlContext = mlContext;
            this.inputDataFileName = inputDataFileName;
            this.console = console;
            multiClassMethod = method;
            binaryClassificationMethod = binaryClassification;
            classificationMethod = prepareClassificationMethod();
        }

        private string prepareClassificationMethod()
        {
            // --- 分類のクラスを準備する
            string methodName = multiClassMethod.ToString() + "[" + binaryClassificationMethod.ToString() + "]";
            try
            {
                if (multiClassMethod == MultiClassMethod.OneVersusAll)
                {
                    switch (binaryClassificationMethod)
                    {
                        case BinaryClassificationMethod.Gam:
                            estimator = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.Gam("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.FastTree:
                            estimator = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.FastTree("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.FastForest:
                            estimator = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.FastForest("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.AveragedPerceptron:
                            estimator = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.AveragedPerceptron("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.LbfgsLogisticRegression:
                            estimator = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.SymbolicSgdLogisticRegression:
                            estimator = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.LightGbm:
                        default:
                            estimator = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.LightGbm("Label", "Features"));
                            break;
                    }
                }
                else // if (multiClassMethod == MultiClassMethod.PairwiseCoupling)
                {
                    switch (binaryClassificationMethod)
                    {
                        case BinaryClassificationMethod.Gam:
                            estimator = mlContext.MulticlassClassification.Trainers.PairwiseCoupling(mlContext.BinaryClassification.Trainers.Gam("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.FastTree:
                            estimator = mlContext.MulticlassClassification.Trainers.PairwiseCoupling(mlContext.BinaryClassification.Trainers.FastTree("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.FastForest:
                            estimator = mlContext.MulticlassClassification.Trainers.PairwiseCoupling(mlContext.BinaryClassification.Trainers.FastForest("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.AveragedPerceptron:
                            estimator = mlContext.MulticlassClassification.Trainers.PairwiseCoupling(mlContext.BinaryClassification.Trainers.AveragedPerceptron("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.LbfgsLogisticRegression:
                            estimator = mlContext.MulticlassClassification.Trainers.PairwiseCoupling(mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.SymbolicSgdLogisticRegression:
                            estimator = mlContext.MulticlassClassification.Trainers.PairwiseCoupling(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression("Label", "Features"));
                            break;
                        case BinaryClassificationMethod.LightGbm:
                        default:
                            estimator = mlContext.MulticlassClassification.Trainers.PairwiseCoupling(mlContext.BinaryClassification.Trainers.LightGbm("Label", "Features"));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] TrainingMultiBinaryClassification " + ex.Message);
                methodName = "???";
                estimator = null;
            }
            return (methodName);
        }

        public bool executeTraining(SensorToUse usePort, string? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData, bool withPresTempHum)
        {
            try
            {
                if (estimator == null)
                {
                    Debug.WriteLine(DateTime.Now + " [ERROR] executeTraining() : estimator is unknown.");
                    console.appendText(DateTime.Now + " [ERROR] executeTraining() : estimator is unknown." + " \r\n\r\n");
                    return (false);
                }
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() START " + classificationMethod + " ----------");
                console.appendText("\r\n ---------- executeTraining() START [" + classificationMethod + "] ----------\r\n");
                console.appendText("   Input File: " + inputDataFileName + "\r\n   Output File: " + outputFileName + " withPresTempHum: " + withPresTempHum + " \r\n\r\n");

                // ----- データの読み込みの設定
                if (withPresTempHum)
                {
                    if (usePort == SensorToUse.port1and2)
                    {
                        dataView = mlContext.Data.LoadFromTextFile<SmellBothData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                        DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                        trainBothSmellData(outputFileName, splitData);
                        if (trainedModel != null)
                        {
                            var testMetrics = mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(splitData.TestSet));
                            showTestMetrics(testMetrics.MicroAccuracy, testMetrics.MacroAccuracy, testMetrics.LogLoss, testMetrics.LogLossReduction);
                        }
                    }
                    else if (usePort == SensorToUse.port1or2)
                    {
                        dataView = mlContext.Data.LoadFromTextFile<SmellOrData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                        DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                        trainOrSmellData(outputFileName, splitData);
                        if (trainedModel != null)
                        {
                            var testMetrics = mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(splitData.TestSet));
                            showTestMetrics(testMetrics.MicroAccuracy, testMetrics.MacroAccuracy, testMetrics.LogLoss, testMetrics.LogLossReduction);
                        }
                    }
                    else if (usePort == SensorToUse.port1)
                    {
                        dataView = mlContext.Data.LoadFromTextFile<SmellData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                        DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                        trainSingle1SmellData(outputFileName, splitData);
                        if (trainedModel != null)
                        {
                            var testMetrics = mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(splitData.TestSet));
                            showTestMetrics(testMetrics.MicroAccuracy, testMetrics.MacroAccuracy, testMetrics.LogLoss, testMetrics.LogLossReduction);
                        }
                    }
                    else if (usePort == SensorToUse.port2)
                    {
                        dataView = mlContext.Data.LoadFromTextFile<SmellData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                        DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                        trainSingle2SmellData(outputFileName, splitData);
                        if (trainedModel != null)
                        {
                            var testMetrics = mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(splitData.TestSet));
                            showTestMetrics(testMetrics.MicroAccuracy, testMetrics.MacroAccuracy, testMetrics.LogLoss, testMetrics.LogLossReduction);
                        }
                    }
                    else
                    {
                        console.appendText(" ----- executeTraining(smell) : unknown port number. ----- \r\n");
                        dataView = null;
                        return (false);
                    }
                }
                else
                {
                    if (usePort == SensorToUse.port1and2)
                    {
                        dataView = mlContext.Data.LoadFromTextFile<OdorBothData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                        DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                        trainBothOdorData(outputFileName, splitData);
                        if (trainedModel != null)
                        {
                            var testMetrics = mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(splitData.TestSet));
                            showTestMetrics(testMetrics.MicroAccuracy, testMetrics.MacroAccuracy, testMetrics.LogLoss, testMetrics.LogLossReduction);
                        }
                    }
                    else if (usePort == SensorToUse.port1or2)
                    {
                        dataView = mlContext.Data.LoadFromTextFile<OdorOrData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                        DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                        trainOrOdorData(outputFileName, splitData);
                        if (trainedModel != null)
                        {
                            var testMetrics = mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(splitData.TestSet));
                            showTestMetrics(testMetrics.MicroAccuracy, testMetrics.MacroAccuracy, testMetrics.LogLoss, testMetrics.LogLossReduction);
                        }
                    }
                    else if (usePort == SensorToUse.port1)
                    {
                        dataView = mlContext.Data.LoadFromTextFile<OdorData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                        DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                        trainSingle1OdorData(outputFileName, splitData);
                        if (trainedModel != null)
                        {
                            var testMetrics = mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(splitData.TestSet));
                            showTestMetrics(testMetrics.MicroAccuracy, testMetrics.MacroAccuracy, testMetrics.LogLoss, testMetrics.LogLossReduction);
                        }
                    }
                    else if (usePort == SensorToUse.port2)
                    {
                        dataView = mlContext.Data.LoadFromTextFile<OdorData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                        DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                        trainSingle2OdorData(outputFileName, splitData);
                        if (trainedModel != null)
                        {
                            var testMetrics = mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(splitData.TestSet));
                            showTestMetrics(testMetrics.MicroAccuracy, testMetrics.MacroAccuracy, testMetrics.LogLoss, testMetrics.LogLossReduction);
                        }
                    }
                    else
                    {
                        console.appendText(" ----- executeTraining() : unknown port number. ----- \r\n");
                        dataView = null;
                        return (false);
                    }
                }
                console.appendText(" ----- executeTraining() : done. ----- \r\n");
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() END  ----------");
                return (true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] executeTraining() " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
                console.appendText(DateTime.Now + " [ERROR] executeTraining() : " + ex.Message + " \r\n\r\n");
            }
            return (false);
        }

        private void showTestMetrics(double microAccuracy, double macroAccuracy, double logLoss, double logLossReduction)
        {
            // ------ モデルの検証結果を画面表示
            //   (参考： https://learn.microsoft.com/en-us/dotnet/machine-learning/tutorials/github-issue-classification )
            console.appendText(" ---------- evaluation result [" + classificationMethod + "] ----------\r\n");
            console.appendText($"   MicroAccuracy   : {microAccuracy:0.###}\r\n");     //  マイクロ精度: 1に近づける
            console.appendText($"   MacroAccuracy   : {macroAccuracy:0.###}\r\n");     //  マクロ精度:   1に近づける
            console.appendText($"   LogLoss         : {logLoss:#.###}\r\n");           //  対数損失:     0に近づける
            console.appendText($"   LogLossReduction: {logLossReduction:#.###}\r\n");  //  対数損失還元: 範囲は [-inf, 1.00] で、1.00に近づける
        }

        private void trainBothSmellData(string? outputFileName, DataOperationsCatalog.TrainTestData testData)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                  .Append(mlContext.Transforms.Concatenate("Features",
                                                          "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                          "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value",
                                                          "sequence10Value", "sequence11Value", "sequence12Value", "sequence13Value", "sequence14Value",
                                                          "sequence15Value", "sequence16Value", "sequence17Value", "sequence18Value", "sequence19Value",
                                                          "averagePressureValue", "averageTemperatureValue", "averageHumidityValue")
                                                          )
                                                  .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(testData.TrainSet);
            if (outputFileName != null)
            {
                mlContext.Model.Save(trainedModel, testData.TrainSet.Schema, outputFileName);
            }
        }

        private void trainOrSmellData(string? outputFileName, DataOperationsCatalog.TrainTestData testData)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                  .Append(mlContext.Transforms.Concatenate("Features", "sensorId",
                                                          "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                          "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value",
                                                          "averagePressureValue", "averageTemperatureValue", "averageHumidityValue"))
                                                  .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(testData.TrainSet);
            if (outputFileName != null)
            {
                mlContext.Model.Save(trainedModel, testData.TrainSet.Schema, outputFileName);
            }
        }

        private void trainSingle1SmellData(string? outputFileName, DataOperationsCatalog.TrainTestData testData)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                   .Append(mlContext.Transforms.Concatenate("Features",
                                                          "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                          "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value",
                                                          "averagePressureValue", "averageTemperatureValue", "averageHumidityValue")
                                                          )
                                                   .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(testData.TrainSet);
            if (outputFileName != null)
            {
                mlContext.Model.Save(trainedModel, testData.TrainSet.Schema, outputFileName);
            }
        }

        private void trainSingle2SmellData(string? outputFileName, DataOperationsCatalog.TrainTestData testData)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                   .Append(mlContext.Transforms.Concatenate("Features",
                                                           "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                           "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value",
                                                           "averagePressureValue", "averageTemperatureValue", "averageHumidityValue")
                                                           )
                                                   .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(testData.TrainSet);
            if (outputFileName != null)
            {
                mlContext.Model.Save(trainedModel, testData.TrainSet.Schema, outputFileName);
            }
        }

        private void trainBothOdorData(string? outputFileName, DataOperationsCatalog.TrainTestData testData)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                  .Append(mlContext.Transforms.Concatenate("Features",
                                                          "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                          "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value",
                                                          "sequence10Value", "sequence11Value", "sequence12Value", "sequence13Value", "sequence14Value",
                                                          "sequence15Value", "sequence16Value", "sequence17Value", "sequence18Value", "sequence19Value"))
                                                  .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(testData.TrainSet);
            if (outputFileName != null)
            {
                mlContext.Model.Save(trainedModel, testData.TrainSet.Schema, outputFileName);
            }
        }

        private void trainOrOdorData(string? outputFileName, DataOperationsCatalog.TrainTestData testData)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                  .Append(mlContext.Transforms.Concatenate("Features", "sensorId",
                                                          "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                          "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value"))
                                                  .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(testData.TrainSet);
            if (outputFileName != null)
            {
                mlContext.Model.Save(trainedModel, testData.TrainSet.Schema, outputFileName);
            }
        }

        private void trainSingle1OdorData(string? outputFileName, DataOperationsCatalog.TrainTestData testData)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                   .Append(mlContext.Transforms.Concatenate("Features",
                                                          "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                          "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value"))
                                                   .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(testData.TrainSet);
            if (outputFileName != null)
            {
                mlContext.Model.Save(trainedModel, testData.TrainSet.Schema, outputFileName);
            }
        }

        private void trainSingle2OdorData(string? outputFileName, DataOperationsCatalog.TrainTestData testData)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                   .Append(mlContext.Transforms.Concatenate("Features",
                                                           "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                           "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value"))
                                                   .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(testData.TrainSet);
            if (outputFileName != null)
            {
                mlContext.Model.Save(trainedModel, testData.TrainSet.Schema, outputFileName);
            }
        }

        public string predictBothData(OdorBothData targetData)
        {
            string result = "???";
            try
            {
                var predictionEngine1and2 = mlContext.Model.CreatePredictionEngine<OdorBothData, PredictionResult>(trainedModel);
                if (predictionEngine1and2 != null)
                {
                    var prediction = predictionEngine1and2.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
                else
                {
                    console.appendText(" [ERROR] predictBothData() : " + " predictionEngine1and2 is null. " + "\r\n");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictBothData() : " + result);
                console.appendText(" [ERROR] predictBothData() : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictOrData(OdorOrData targetData)
        {
            string result = "???";
            try
            {
                var predictionEngine1or2 = mlContext.Model.CreatePredictionEngine<OdorOrData, PredictionResult>(trainedModel);
                if (predictionEngine1or2 != null)
                {
                    var prediction = predictionEngine1or2.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
                else
                {
                    console.appendText(" [ERROR] predictOrData() : " + " predictionEngine1or2 is null. " + "\r\n");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictOrData() : " + result);
                console.appendText(" [ERROR] predictOrData() : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictSingle1Data(OdorData targetData)
        {
            string result = "???";
            try
            {
                var predictionEngineSingle1 = mlContext.Model.CreatePredictionEngine<OdorData, PredictionResult>(trainedModel);
                if (predictionEngineSingle1 != null)
                {
                    var prediction = predictionEngineSingle1.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
                else
                {
                    console.appendText(" [ERROR] predictSingle1Data() : " + " predictionEngineSingle1 is null. " + "\r\n");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictSingle1Data() : " + result);
                console.appendText(" [ERROR] predictSingle1Data() : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictSingle2Data(OdorData targetData)
        {
            string result = "???";
            try
            {
                var predictionEngineSingle2 = mlContext.Model.CreatePredictionEngine<OdorData, PredictionResult>(trainedModel);
                if (predictionEngineSingle2 != null)
                {
                    var prediction = predictionEngineSingle2.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
                else
                {
                    console.appendText(" [ERROR] predictSingle2Data() : " + " predictionEngineSingle2 is null. " + "\r\n");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictSingle2Data() : " + result);
                console.appendText(" [ERROR] predictSingle2Data() : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictBothData(SmellBothData targetData)
        {
            string result = "???";
            try
            {
                var predictionEngine1and2 = mlContext.Model.CreatePredictionEngine<SmellBothData, PredictionResult>(trainedModel);
                if (predictionEngine1and2 != null)
                {
                    var prediction = predictionEngine1and2.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
                else
                {
                    console.appendText(" [ERROR] predictBothData(smell) : " + " predictionEngine1and2 is null. " + "\r\n");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictBothData(smell) : " + result);
                console.appendText(" [ERROR] predictBothData(smell) : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictOrData(SmellOrData targetData)
        {
            string result = "???";
            try
            {
                var predictionEngine1or2 = mlContext.Model.CreatePredictionEngine<SmellOrData, PredictionResult>(trainedModel);
                if (predictionEngine1or2 != null)
                {
                    var prediction = predictionEngine1or2.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
                else
                {
                    console.appendText(" [ERROR] predictOrData(smell) : " + " predictionEngine1or2 is null. " + "\r\n");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictOrData(smell) : " + result);
                console.appendText(" [ERROR] predictOrData(smell) : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictSingle1Data(SmellData targetData)
        {
            string result = "???";
            try
            {
                var predictionEngineSingle1 = mlContext.Model.CreatePredictionEngine<SmellData, PredictionResult>(trainedModel);
                if (predictionEngineSingle1 != null)
                {
                    var prediction = predictionEngineSingle1.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
                else
                {
                    console.appendText(" [ERROR] predictSingle1Data(smell) : " + " predictionEngineSingle1 is null. " + "\r\n");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictSingle1Data(smell) : " + result);
                console.appendText(" [ERROR] predictSingle1Data(smell) : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictSingle2Data(SmellData targetData)
        {
            string result = "???";
            try
            {
                var predictionEngineSingle2 = mlContext.Model.CreatePredictionEngine<SmellData, PredictionResult>(trainedModel);
                if (predictionEngineSingle2 != null)
                {
                    var prediction = predictionEngineSingle2.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
                else
                {
                    console.appendText(" [ERROR] predictSingle2Data(smell) : " + " predictionEngineSingle2 is null. " + "\r\n");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictSingle2Data(smell) : " + result);
                console.appendText(" [ERROR] predictSingle2Data(smell) : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public bool savePredictionModel(string outputFileName)
        {
            bool ret = false;
            try
            {
                if ((trainedModel != null) && (dataView != null))
                {
                    DataViewSchema modelSchema = dataView.Schema;
                    mlContext.Model.Save(trainedModel, modelSchema, outputFileName);
                    ret = true;
                }
                else
                {
                    Debug.WriteLine(DateTime.Now + " [ERROR] savePredictionModel() : The model does not exist.");
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] savePredictionModel() : " + ee.Message);
                ret = false;
            }
            return (ret);
        }

        public string getMethodName()
        {
            return (classificationMethod);
        }
    }
}
