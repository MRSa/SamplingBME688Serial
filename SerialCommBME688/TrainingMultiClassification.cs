using Microsoft.ML;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace SamplingBME688Serial
{
    class TrainingMultiClassification : ICreatePredictionModel, IPredictionModel
    {
        private MLContext mlContext;
        private String inputDataFileName;
        private String classificationMethod;
        private IEstimator<ITransformer> estimator;
        private ICreateModelConsole console;
        private ITransformer? trainedModel = null;
        private IDataView? dataView = null;

        public TrainingMultiClassification(ref MLContext mlContext, String classificationMethod, ref IEstimator<ITransformer> estimator, String inputDataFileName, ICreateModelConsole console)
        {
            this.mlContext = mlContext;
            this.classificationMethod = classificationMethod;
            this.estimator = estimator;
            this.inputDataFileName = inputDataFileName;
            this.console = console;
        }

        public bool executeTraining(SensorToUse usePort, String? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            try
            {
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() START " + classificationMethod + " ----------");
                console.appendText(" ----- executeTraining() " + classificationMethod + " Input File: " + inputDataFileName + "  Output File: " + outputFileName + " START -----\r\n");

                // ----- データの読み込みの設定
                if (usePort == SensorToUse.port1and2)
                {
                    dataView = mlContext.Data.LoadFromTextFile<OdorBothData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                    trainBothOdorData(outputFileName, splitData);
                }
                else if (usePort == SensorToUse.port1or2)
                {
                    dataView = mlContext.Data.LoadFromTextFile<OdorOrData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                    trainOrOdorData(outputFileName, splitData);
                }
                else if (usePort == SensorToUse.port1)
                {
                    dataView = mlContext.Data.LoadFromTextFile<OdorData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                    trainSingle1OdorData(outputFileName, splitData);
                }
                else if (usePort == SensorToUse.port2)
                {
                    dataView = mlContext.Data.LoadFromTextFile<OdorData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    DataOperationsCatalog.TrainTestData splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                    trainSingle2OdorData(outputFileName, splitData);
                }
                else
                {
                    dataView = null;
                    console.appendText(" ----- executeTraining() : unknown port number.\r\n");
                    return (false);
                }
                console.appendText(" ----- executeTraining() : done.\r\n");
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

        private void trainBothOdorData(String? outputFileName, DataOperationsCatalog.TrainTestData testData)
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

        private void trainOrOdorData(String? outputFileName, DataOperationsCatalog.TrainTestData testData)
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

        private void trainSingle1OdorData(String? outputFileName, DataOperationsCatalog.TrainTestData testData)
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

        private void trainSingle2OdorData(String? outputFileName, DataOperationsCatalog.TrainTestData testData)
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

        public String predictBothData(OdorBothData targetData)
        {
            String result = "???";
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

        public String predictOrData(OdorOrData targetData)
        {
            String result = "???";
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

        public String predictSingle1Data(OdorData targetData)
        {
            String result = "???";
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

        public String predictSingle2Data(OdorData targetData)
        {
            String result = "???";
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

        public bool savePredictionModel(String outputFileName)
        {
            bool ret = false;
            try
            {
                if ((trainedModel != null)&&(dataView != null))
                {
                    DataViewSchema modelSchema = dataView.Schema;
                    mlContext.Model.Save(trainedModel, modelSchema, outputFileName);
                    ret = true;
                }
                else
                {
                    Debug.WriteLine(DateTime.Now + " [ERROR] savePredictionModel() : The model does not exist." );
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] savePredictionModel() : " + ee.Message);
                ret = false;
            }
            return (ret);
        }
    }
}
