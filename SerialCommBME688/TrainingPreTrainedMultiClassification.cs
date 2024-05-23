using Microsoft.ML;
using System.Diagnostics;

namespace SamplingBME688Serial
{
    class TrainingPreTrainedMultiClassification : IPredictionModel
    {
        private MLContext mlContext;
        private ICreateModelConsole console;
        private ITransformer? trainedModel = null;

        public TrainingPreTrainedMultiClassification(ref MLContext mlContext, ICreateModelConsole console)
        {
            this.mlContext = mlContext;
            this.console = console;
        }

        public bool loadPreTrainedModel(string inputFileName)
        {
            bool ret = false;
            try
            {
                DataViewSchema modelSchema;
                trainedModel = mlContext.Model.Load(inputFileName, out modelSchema);
                console.appendText("[Info] load pre-trained model : " + inputFileName + "\r\n");
                ret = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " loadPredictionModel : " + inputFileName + "  " + ex.Message);
                console.appendText(" [Error] load pre-trained model : " + inputFileName + "\r\n");
                trainedModel = null;
            }
            return (ret);
        }

        public bool savePredictionModel(string outputFileName)
        {
            // ----- ロードしたモデルの場合は、保存ができません
            // MessageBox.Show("The pre-trained model can not be saved.", "Save cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Debug.WriteLine(DateTime.Now + " The pre-trained model cannot not save. : " + outputFileName);
            console.appendText(" [Error] The pre-trained model can not be saved. : " + outputFileName + "\r\n");
            return (false);
        }

        public string predictBothData(OdorBothData targetData)
        {
            string result = "???";
            if (trainedModel == null)
            {
                return ("(no model)");
            }
            try
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<OdorBothData, PredictionResult>(trainedModel);
                if (predictionEngine != null)
                {
                    var prediction = predictionEngine.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [Error] predictBothData() : " + ex.Message);
                console.appendText(" [Error] predictBothData() : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictOrData(OdorOrData targetData)
        {
            string result = "???";
            if (trainedModel == null)
            {
                return ("(no model)");
            }
            try
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<OdorOrData, PredictionResult>(trainedModel);
                if (predictionEngine != null)
                {
                    var prediction = predictionEngine.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [Error] predictOrData() : " + ex.Message);
                console.appendText(" [Error] predictOrData() : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictSingle1Data(OdorData targetData)
        {
            string result = "???";
            if (trainedModel == null)
            {
                return ("(no model)");
            }
            try
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<OdorData, PredictionResult>(trainedModel);
                if (predictionEngine != null)
                {
                    var prediction = predictionEngine.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [Error] predictSingle1Data() : " + ex.Message);
                console.appendText(" [Error] predictSingle1Data() : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictSingle2Data(OdorData targetData)
        {
            string result = "???";
            if (trainedModel == null)
            {
                return ("(no model)");
            }
            try
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<OdorData, PredictionResult>(trainedModel);
                if (predictionEngine != null)
                {
                    var prediction = predictionEngine.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [Error] predictSingle2Data() : " + ex.Message);
                console.appendText(" [Error] predictSingle2Data() : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictBothData(SmellBothData targetData)
        {
            string result = "???";
            if (trainedModel == null)
            {
                return ("(no model)");
            }
            try
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<SmellBothData, PredictionResult>(trainedModel);
                if (predictionEngine != null)
                {
                    var prediction = predictionEngine.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [Error] predictBothData(smell) : " + ex.Message);
                console.appendText(" [Error] predictBothData(smell) : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictOrData(SmellOrData targetData)
        {
            string result = "???";
            if (trainedModel == null)
            {
                return ("(no model)");
            }
            try
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<SmellOrData, PredictionResult>(trainedModel);
                if (predictionEngine != null)
                {
                    var prediction = predictionEngine.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [Error] predictOrData(smell) : " + ex.Message);
                console.appendText(" [Error] predictOrData(smell) : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictSingle1Data(SmellData targetData)
        {
            string result = "???";
            if (trainedModel == null)
            {
                return ("(no model)");
            }
            try
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<SmellData, PredictionResult>(trainedModel);
                if (predictionEngine != null)
                {
                    var prediction = predictionEngine.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [Error] predictSingle1Data(smell) : " + ex.Message);
                console.appendText(" [Error] predictSingle1Data(smell) : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string predictSingle2Data(SmellData targetData)
        {
            string result = "???";
            if (trainedModel == null)
            {
                return ("(no model)");
            }
            try
            {
                var predictionEngine = mlContext.Model.CreatePredictionEngine<SmellData, PredictionResult>(trainedModel);
                if (predictionEngine != null)
                {
                    var prediction = predictionEngine.Predict(targetData);
                    if ((prediction != null) && (prediction.dataLabel != null))
                    {
                        result = prediction.dataLabel;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [Error] predictSingle2Data(smell) : " + ex.Message);
                console.appendText(" [Error] predictSingle2Data(smell) : " + ex.Message + "\r\n");
            }
            return (result);
        }

        public string getMethodName()
        {
            return ("Pre-Trained");
        }
    }
}
