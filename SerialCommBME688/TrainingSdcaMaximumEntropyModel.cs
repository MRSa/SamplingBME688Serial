using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamplingBME688Serial
{
    class TrainingSdcaMaximumEntropyModel : ICreatePredictionModel, IPredictionModel
    {
        private MLContext mlContext;
        ITransformer trainedModel;

        PredictionEngine<OdorBothData, PredictionResult> predictionEngine1and2;
        PredictionEngine<OdorOrData, PredictionResult> predictionEngine1or2;
        PredictionEngine<OdorData, PredictionResult> predictionEngineSingle1;
        PredictionEngine<OdorData, PredictionResult> predictionEngineSingle2;

        private ICreateModelConsole console;
        private String inputDataFileName;
        private String validationDataFileName;
        private bool doneTraining = false;

        public TrainingSdcaMaximumEntropyModel(ref MLContext mlContext, String inputDataFileName, String validationDataFileName, ICreateModelConsole console)
        {
            this.mlContext = mlContext;
            this.inputDataFileName = inputDataFileName;
            this.validationDataFileName = validationDataFileName;
            this.console = console;
        }

        private IEstimator<ITransformer> ProcessData1and2()
        {
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                .Append(mlContext.Transforms.Concatenate("Features",
                                                         "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                         "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value",
                                                         "sequence10Value", "sequence11Value", "sequence12Value", "sequence13Value", "sequence14Value",
                                                         "sequence15Value", "sequence16Value", "sequence17Value", "sequence18Value", "sequence19Value"))
                .AppendCacheCheckpoint(mlContext);

            return pipeline;
        }

        private IEstimator<ITransformer> BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            var trainingPipeline = pipeline.Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            trainedModel = trainingPipeline.Fit(trainingDataView);
            predictionEngine1and2 = mlContext.Model.CreatePredictionEngine<OdorBothData, PredictionResult>(trainedModel);

            return (trainingPipeline);
        }


        public bool executeTraining(SensorToUse usePort, String? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            try
            {                
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() START " + "SdcaMaximumEntropy" + " ----------");
                console.appendText("executeTraining() SdcaMaximumEntropy " + " Output File: " + outputFileName + "\r\n");

                // ----- データの読み込みの設定
                if (usePort == SensorToUse.port1and2)
                {
                    trainBothOdorData(outputFileName, ref port1, ref port2, isLogData);
                }
                else if (usePort == SensorToUse.port1or2)
                {
                    trainOrOdorData(outputFileName, ref port1, ref port2, isLogData);
                }
                else if (usePort == SensorToUse.port1)
                {
                    trainSingle1OdorData(outputFileName, ref port1, isLogData);
                }
                else if (usePort == SensorToUse.port2)
                {
                    trainSingle2OdorData(outputFileName, ref port2, isLogData);
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

        private void trainBothOdorData(String? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<OdorBothData>(inputDataFileName, hasHeader: false, separatorChar: ',');
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                .Append(mlContext.Transforms.Concatenate("Features",
                                                         "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                         "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value",
                                                         "sequence10Value", "sequence11Value", "sequence12Value", "sequence13Value", "sequence14Value",
                                                         "sequence15Value", "sequence16Value", "sequence17Value", "sequence18Value", "sequence19Value"))
                .AppendCacheCheckpoint(mlContext);

            var trainingPipeline = pipeline.Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(dataView);
            predictionEngine1and2 = mlContext.Model.CreatePredictionEngine<OdorBothData, PredictionResult>(trainedModel);

            testBoth();

        }

        private void trainOrOdorData(String? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<OdorOrData>(inputDataFileName, hasHeader: false, separatorChar: ',');
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                .Append(mlContext.Transforms.Concatenate("Features", "sensorId",
                                                         "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                         "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value"))
                .AppendCacheCheckpoint(mlContext);

            var trainingPipeline = pipeline.Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
               .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(dataView);
            predictionEngine1or2 = mlContext.Model.CreatePredictionEngine<OdorOrData, PredictionResult>(trainedModel);

            testOr();
        }

        private void trainSingle1OdorData(String? outputFileName, ref IDataHolder? port, bool isLogData)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<OdorData>(inputDataFileName, hasHeader: false, separatorChar: ',');
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                .Append(mlContext.Transforms.Concatenate("Features",
                                                         "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                         "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value"))
                .AppendCacheCheckpoint(mlContext);

            var trainingPipeline = pipeline.Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
               .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(dataView);
            predictionEngineSingle1 = mlContext.Model.CreatePredictionEngine<OdorData, PredictionResult>(trainedModel);

            testSingle1();
        }

        private void trainSingle2OdorData(String? outputFileName, ref IDataHolder? port, bool isLogData)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<OdorData>(inputDataFileName, hasHeader: false, separatorChar: ',');
            IEstimator<ITransformer> pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                .Append(mlContext.Transforms.Concatenate("Features",
                                                         "sequence0Value", "sequence1Value", "sequence2Value", "sequence3Value", "sequence4Value",
                                                         "sequence5Value", "sequence6Value", "sequence7Value", "sequence8Value", "sequence9Value"))
                .AppendCacheCheckpoint(mlContext);

            var trainingPipeline = pipeline.Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
               .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            trainedModel = trainingPipeline.Fit(dataView);
            predictionEngineSingle2 = mlContext.Model.CreatePredictionEngine<OdorData, PredictionResult>(trainedModel);

            testSingle2();
        }


        private void testBoth()
        {
            try
            {
                TestSampleData testData = new TestSampleData();
                var prediction1 = predictionEngine1and2.Predict(testData.getBothData1());
                var prediction2 = predictionEngine1and2.Predict(testData.getBothData2());
                var prediction3 = predictionEngine1and2.Predict(testData.getBothData3());
                var prediction4 = predictionEngine1and2.Predict(testData.getBothData4());

                String outputMessage = $"Prediction1: {prediction1.dataLabel}, Score: ";
                if (prediction1.Distances?.Length > 0)
                {
                    foreach (var data in prediction1.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction1: {prediction1.dataLabel}, " + $"Score: {prediction1.Distances?.Length}");
                }

                outputMessage = $"Prediction2: {prediction2.dataLabel}, Score: ";
                if (prediction2.Distances?.Length > 0)
                {
                    foreach (var data in prediction2.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction2: {prediction2.dataLabel}, " + $"Score: {prediction2.Distances?.Length}");
                }

                outputMessage = $"Prediction3: {prediction3.dataLabel}, Score: ";
                if (prediction3.Distances?.Length > 0)
                {
                    foreach (var data in prediction3.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction3: {prediction3.dataLabel}, " + $"Score: {prediction3.Distances?.Length}");
                }

                outputMessage = $"Prediction4: {prediction4.dataLabel}, Score: ";
                if (prediction4.Distances?.Length > 0)
                {
                    foreach (var data in prediction4.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction4: {prediction4.dataLabel}, " + $"Score: {prediction4.Distances?.Length}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] testBoth() " + ex.Message);
            }
        }

        private void testOr()
        {
            try
            {
                TestSampleData testData = new TestSampleData();
                var prediction1 = predictionEngine1or2.Predict(testData.getOdorOr1Data1());
                var prediction2 = predictionEngine1or2.Predict(testData.getOdorOr1Data2());
                var prediction3 = predictionEngine1or2.Predict(testData.getOdorOr1Data3());
                var prediction4 = predictionEngine1or2.Predict(testData.getOdorOr1Data3());

                String outputMessage = $"Prediction1: {prediction1.dataLabel}, Score: ";
                if (prediction1.Distances?.Length > 0)
                {
                    foreach (var data in prediction1.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction1: {prediction1.dataLabel}, " + $"Score: {prediction1.Distances?.Length}");
                }

                outputMessage = $"Prediction2: {prediction2.dataLabel}, Score: ";
                if (prediction2.Distances?.Length > 0)
                {
                    foreach (var data in prediction2.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction2: {prediction2.dataLabel}, " + $"Score: {prediction2.Distances?.Length}");
                }

                outputMessage = $"Prediction3: {prediction3.dataLabel}, Score: ";
                if (prediction3.Distances?.Length > 0)
                {
                    foreach (var data in prediction3.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction3: {prediction3.dataLabel}, " + $"Score: {prediction3.Distances?.Length}");
                }

                outputMessage = $"Prediction4: {prediction4.dataLabel}, Score: ";
                if (prediction4.Distances?.Length > 0)
                {
                    foreach (var data in prediction4.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction4: {prediction4.dataLabel}, " + $"Score: {prediction4.Distances?.Length}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] testBoth() " + ex.Message);
            }
        }

        private void testSingle1()
        {
            try
            {
                TestSampleData testData = new TestSampleData();
                var prediction1 = predictionEngineSingle1.Predict(testData.getOdor1Data1());
                var prediction2 = predictionEngineSingle1.Predict(testData.getOdor1Data2());
                var prediction3 = predictionEngineSingle1.Predict(testData.getOdor1Data3());
                var prediction4 = predictionEngineSingle1.Predict(testData.getOdor1Data4());

                String outputMessage = $"Prediction1: {prediction1.dataLabel}, Score: ";
                if (prediction1.Distances?.Length > 0)
                {
                    foreach (var data in prediction1.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction1: {prediction1.dataLabel}, " + $"Score: {prediction1.Distances?.Length}");
                }

                outputMessage = $"Prediction2: {prediction2.dataLabel}, Score: ";
                if (prediction2.Distances?.Length > 0)
                {
                    foreach (var data in prediction2.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction2: {prediction2.dataLabel}, " + $"Score: {prediction2.Distances?.Length}");
                }

                outputMessage = $"Prediction3: {prediction3.dataLabel}, Score: ";
                if (prediction3.Distances?.Length > 0)
                {
                    foreach (var data in prediction3.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction3: {prediction3.dataLabel}, " + $"Score: {prediction3.Distances?.Length}");
                }

                outputMessage = $"Prediction4: {prediction4.dataLabel}, Score: ";
                if (prediction4.Distances?.Length > 0)
                {
                    foreach (var data in prediction4.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction4: {prediction4.dataLabel}, " + $"Score: {prediction4.Distances?.Length}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] testBoth() " + ex.Message);
            }
        }

        private void testSingle2()
        {
            try
            {
                TestSampleData testData = new TestSampleData();
                var prediction1 = predictionEngineSingle2.Predict(testData.getOdor2Data1());
                var prediction2 = predictionEngineSingle2.Predict(testData.getOdor2Data2());
                var prediction3 = predictionEngineSingle2.Predict(testData.getOdor2Data3());
                var prediction4 = predictionEngineSingle2.Predict(testData.getOdor2Data4());

                String outputMessage = $"Prediction1: {prediction1.dataLabel}, Score: ";
                if (prediction1.Distances?.Length > 0)
                {
                    foreach (var data in prediction1.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction1: {prediction1.dataLabel}, " + $"Score: {prediction1.Distances?.Length}");
                }

                outputMessage = $"Prediction2: {prediction2.dataLabel}, Score: ";
                if (prediction2.Distances?.Length > 0)
                {
                    foreach (var data in prediction2.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction2: {prediction2.dataLabel}, " + $"Score: {prediction2.Distances?.Length}");
                }

                outputMessage = $"Prediction3: {prediction3.dataLabel}, Score: ";
                if (prediction3.Distances?.Length > 0)
                {
                    foreach (var data in prediction3.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction3: {prediction3.dataLabel}, " + $"Score: {prediction3.Distances?.Length}");
                }

                outputMessage = $"Prediction4: {prediction4.dataLabel}, Score: ";
                if (prediction4.Distances?.Length > 0)
                {
                    foreach (var data in prediction4.Distances)
                    {
                        outputMessage += data.ToString() + " ";
                    }
                    Debug.WriteLine(outputMessage);
                }
                else
                {
                    Debug.WriteLine($"Prediction4: {prediction4.dataLabel}, " + $"Score: {prediction4.Distances?.Length}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] testBoth() " + ex.Message);
            }
        }



        public String predictBothData(OdorBothData targetData)
        {
            String result = "";
            try
            {

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
            String result = "";
            try
            {

            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictOrData() : " + result);
            }
            return (result);
        }


        public String predictSingleData(OdorData targetData)
        {
            String result = "";
            try
            {

            }
            catch (Exception ex)
            {
                result = ex.Message;
                Debug.WriteLine(DateTime.Now + " [ERROR] predictSingleData() : " + result);
            }
            return (result);
        }
    }
}
