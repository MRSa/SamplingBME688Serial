using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    class TrainingNaiveBayesModel : ICreatePredictionModel, IPredictionModel
    {
        private MLContext mlContext;
        private ICreateModelConsole console;
        private String inputDataFileName;
        private String validationDataFileName;
        private bool doneTraining = false;

        public TrainingNaiveBayesModel(ref MLContext mlContext, String inputDataFileName, String validationDataFileName, ICreateModelConsole console)
        {
            this.mlContext = mlContext;
            this.inputDataFileName = inputDataFileName;
            this.validationDataFileName = validationDataFileName;
            this.console = console;
        }

        public bool executeTraining(SensorToUse usePort, String? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData)
        {
            try
            {
                console.appendText("CREATE LABEL\r\n");
                switch (usePort)
                {
                    case SensorToUse.port1and2:
                        // ----- 両方のポートを同時に使用する場合
                        trainBothOdorData(outputFileName, ref port1, ref port2, isLogData);
                        break;
                    case SensorToUse.port1or2:
                        // ----- 両方のポートを片方づつ使用する場合
                        trainOrOdorData(outputFileName, ref port1, ref port2, isLogData);
                        break;
                    case SensorToUse.port1:
                        // ----- ポート１のみ使用する場合
                        trainSingleOdorData(outputFileName, ref port1, isLogData);
                        break;
                    case SensorToUse.port2:
                        // ----- ポート２のみ使用する場合
                        trainSingleOdorData(outputFileName, ref port2, isLogData);
                        break;
                    case SensorToUse.None:
                    default:
                        // ----- 使用ポートが不明...ラベルなしとする
                        console.appendText("  UNKNOWN sensor Type : LABEL is NONE.\r\n");
                        break;
                }
                console.appendText(" ----- executeTraining() : done.\r\n");
                Debug.WriteLine(DateTime.Now + " ---------- executeTraining() END  ----------");
 
                doneTraining = true;
                return (true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] executeTraining() " + ex.Message);
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
