using Microsoft.ML;
using System.Diagnostics;

namespace SamplingBME688Serial
{
    class TrainingMulticlassClassificationBase : ICreatePredictionModel, IPredictionModel
    {
        private MLContext mlContext;
        private ITransformer? trainedModel;
        private IEstimator<ITransformer> estimator;

        private PredictionEngine<OdorBothData, PredictionResult>? predictionEngine1and2;
        private PredictionEngine<OdorOrData, PredictionResult>? predictionEngine1or2;
        private PredictionEngine<OdorData, PredictionResult>? predictionEngineSingle1;
        private PredictionEngine<OdorData, PredictionResult>? predictionEngineSingle2;

        private ICreateModelConsole console;
        private String inputDataFileName;
        private String classificationMethod;
        private bool doneTraining = false;

        public TrainingMulticlassClassificationBase(ref MLContext mlContext, String classificationMethod, ref IEstimator<ITransformer> estimator, String inputDataFileName, ICreateModelConsole console)
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
                // IEstimator<ITransformer> estimator = mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features");

                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() START " + classificationMethod + " ----------");
                console.appendText("executeTraining() SdcaMaximumEntropy " + " Output File: " + outputFileName + "\r\n");

                IDataView dataView = mlContext.Data.LoadFromTextFile<OdorBothData>(inputDataFileName, hasHeader: false, separatorChar: ',');

                // ----- データの読み込みの設定
                if (usePort == SensorToUse.port1and2)
                {
                    trainBothOdorData(outputFileName, dataView);
                }
                else if (usePort == SensorToUse.port1or2)
                {
                    trainOrOdorData(outputFileName, dataView);
                }
                else if (usePort == SensorToUse.port1)
                {
                    trainSingle1OdorData(outputFileName, dataView);
                }
                else if (usePort == SensorToUse.port2)
                {
                    trainSingle2OdorData(outputFileName, dataView);
                }
                else
                {
                    console.appendText(" ----- executeTraining() : unknown port number.\r\n");
                    doneTraining = false;
                    return (false);
                }
                console.appendText(" ----- executeTraining() : done.\r\n");
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() END  ----------");

                doneTraining = true;
                return (true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] executeTraining() " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
                console.appendText(DateTime.Now + " [ERROR] executeTraining() : " + ex.Message + " \r\n\r\n");
            }
            doneTraining = false;
            return (false);
        }

        private void trainBothOdorData(String? outputFileName, IDataView dataView)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                  .Append(mlContext.Transforms.Concatenate("Features",
                                                          "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                          "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value",
                                                          "sequence10Value", "sequence11Value", "sequence12Value", "sequence13Value", "sequence14Value",
                                                          "sequence15Value", "sequence16Value", "sequence17Value", "sequence18Value", "sequence19Value"))
                                                  .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            var trainedModel0 = trainingPipeline.Fit(dataView);
            predictionEngine1and2 = mlContext.Model.CreatePredictionEngine<OdorBothData, PredictionResult>(trainedModel0);
        }

        private void trainOrOdorData(String? outputFileName, IDataView dataView)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                  .Append(mlContext.Transforms.Concatenate("Features", "sensorId",
                                                          "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                          "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value"))
                                                  .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            var trainedModel1 = trainingPipeline.Fit(dataView);
            predictionEngine1or2 = mlContext.Model.CreatePredictionEngine<OdorOrData, PredictionResult>(trainedModel1);
        }

        private void trainSingle1OdorData(String? outputFileName, IDataView dataView)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                   .Append(mlContext.Transforms.Concatenate("Features",
                                                          "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                          "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value"))
                                                   .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            var trainedModel2 = trainingPipeline.Fit(dataView);
            predictionEngineSingle1 = mlContext.Model.CreatePredictionEngine<OdorData, PredictionResult>(trainedModel2);
        }

        private void trainSingle2OdorData(String? outputFileName, IDataView dataView)
        {
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                                                   .Append(mlContext.Transforms.Concatenate("Features",
                                                           "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                           "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value"))
                                                   .AppendCacheCheckpoint(mlContext);
            var trainingPipeline = pipeline.Append(estimator).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            var trainedModel3 = trainingPipeline.Fit(dataView);
            predictionEngineSingle2 = mlContext.Model.CreatePredictionEngine<OdorData, PredictionResult>(trainedModel3);
        }

        public String predictBothData(OdorBothData targetData)
        {
            String result = "???";
            try
            {
                if (predictionEngine1and2 != null)
                {
                    var prediction = predictionEngine1and2.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictBothData() : " + result);
            }
            return (result);
        }

        public String predictOrData(OdorOrData targetData)
        {
            String result = "???";
            try
            {
                if (predictionEngine1or2 != null)
                {
                    var prediction = predictionEngine1or2.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictOrData() : " + result);
            }
            return (result);
        }

        public String predictSingle1Data(OdorData targetData)
        {
            String result = "???";
            try
            {
                if (predictionEngineSingle1 != null)
                {
                    var prediction = predictionEngineSingle1.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictSingle1Data() : " + result);
            }
            return (result);
        }

        public String predictSingle2Data(OdorData targetData)
        {
            String result = "???";
            try
            {
                if (predictionEngineSingle2 != null)
                {
                    var prediction = predictionEngineSingle2.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictSingle2Data() : " + result);
            }
            return (result);
        }
    }
}
