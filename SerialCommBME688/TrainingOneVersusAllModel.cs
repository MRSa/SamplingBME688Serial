using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamplingBME688Serial
{
    class TrainingOneVersusAllModel : ICreatePredictionModel, IPredictionModel
    {
        private MLContext mlContext;
        ITransformer trainedModel;
        PredictionEngine<OdorBothData, PredictionResult> predictionEngine1and2;
        private ICreateModelConsole console;
        private String inputDataFileName;
        private String validationDataFileName;
        private bool doneTraining = false;
        private Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.MulticlassPredictionTransformer<Microsoft.ML.Trainers.OneVersusAllModelParameters>>? model = null;

        public TrainingOneVersusAllModel(ref MLContext mlContext, String inputDataFileName, String validationDataFileName, ICreateModelConsole console)
        {
            this.mlContext = mlContext;
            this.inputDataFileName = inputDataFileName;
            this.validationDataFileName = validationDataFileName;
            this.console = console;
        }

        private IEstimator<ITransformer> ProcessData1and2()
        {
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "dataLabel", outputColumnName: "Label")
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence0Value", outputColumnName: "Featurized0"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence1Value", outputColumnName: "Featurized1"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence2Value", outputColumnName: "Featurized2"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence3Value", outputColumnName: "Featurized3"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence4Value", outputColumnName: "Featurized4"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence5Value", outputColumnName: "Featurized5"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence6Value", outputColumnName: "Featurized6"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence7Value", outputColumnName: "Featurized7"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence8Value", outputColumnName: "Featurized8"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence9Value", outputColumnName: "Featurized9"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence10Value", outputColumnName: "Featurized10"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence11Value", outputColumnName: "Featurized11"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence12Value", outputColumnName: "Featurized12"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence13Value", outputColumnName: "Featurized13"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence14Value", outputColumnName: "Featurized14"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence15Value", outputColumnName: "Featurized15"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence16Value", outputColumnName: "Featurized16"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence17Value", outputColumnName: "Featurized17"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence18Value", outputColumnName: "Featurized18"))
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "sequence19Value", outputColumnName: "Featurized19"))
                .Append(mlContext.Transforms.Concatenate("Features",
                                                         "Featurized0", "Featurized1", "Featurized2", "Featurized3", "Featurized4",
                                                         "Featurized5", "Featurized6", "Featurized7", "Featurized8", "Featurized9",
                                                         "Featurized10", "Featurized11", "Featurized12", "Featurized13", "Featurized14",
                                                         "Featurized15", "Featurized16", "Featurized17", "Featurized18", "Featurized19"))
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
                IDataView dataView;
                IEstimator<ITransformer> pipeline; 
                
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() START " + "One versus All" + " ----------");
                console.appendText("executeTraining() [1:*] " + " Output File: " + outputFileName + "\r\n");

                // ----- データの読み込みの設定
                if (usePort == SensorToUse.port1and2)
                {
                    dataView = mlContext.Data.LoadFromTextFile<OdorBothData>(inputDataFileName, hasHeader: false, separatorChar: ',');
                    pipeline = ProcessData1and2();

                    var trainingPipeline = BuildAndTrainModel(dataView, pipeline);

                    TestSampleData testData = new TestSampleData();
                    var prediction1 = predictionEngine1and2.Predict(testData.getBothData1());
                    var prediction2 = predictionEngine1and2.Predict(testData.getBothData1());
                    var prediction3 = predictionEngine1and2.Predict(testData.getBothData1());
                    var prediction4 = predictionEngine1and2.Predict(testData.getBothData1());

                    Debug.WriteLine($"Prediction1: {prediction1.dataLabel}, " + $"Score: {prediction1.Distances?.Length}");
                    Debug.WriteLine($"Prediction2: {prediction2.dataLabel}, " + $"Score: {prediction2.Distances?.Length}");
                    Debug.WriteLine($"Prediction3: {prediction3.dataLabel}, " + $"Score: {prediction3.Distances?.Length}");
                    Debug.WriteLine($"Prediction4: {prediction4.dataLabel}, " + $"Score: {prediction4.Distances?.Length}");
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

        }

        private void trainOrOdorData(String? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {

        }

        private void trainSingleOdorData(String? outputFileName, ref IDataHolder? port, bool isLogData)
        {

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
