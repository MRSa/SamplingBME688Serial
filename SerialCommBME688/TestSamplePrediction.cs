using System.Diagnostics;

namespace SamplingBME688Serial
{
    internal class TestSamplePrediction
    {
        public String testPrediction(SensorToUse usePort, ref IPredictionModel? predictionModel, bool isLogData)
        {
            String result = "";
            try
            {
                if (predictionModel == null)
                {
                    Debug.WriteLine(DateTime.Now + " [ERROR] testPrediction() : predictionModel is null. ");
                    return ("");
                }
                if (isLogData)
                {
                    result = testTrainingResultLog(usePort, ref predictionModel);
                }
                else
                {
                    result = testTrainingResult(usePort, ref predictionModel);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] testPrediction() " + ex.Message);
            }
            return (result);
        }

        private String testTrainingResultLog(SensorToUse usePort, ref IPredictionModel predictionModel)
        {
            String result = "";
            try
            {
                 TestSampleData sampleData = new TestSampleData();
                if (usePort == SensorToUse.port1and2)
                {
                    result += predictionModel.predictBothData(sampleData.getBothData1Log());
                    result += " ";
                    result += predictionModel.predictBothData(sampleData.getBothData2Log());
                    result += " ";
                    result += predictionModel.predictBothData(sampleData.getBothData3Log());
                    result += " ";
                    result += predictionModel.predictBothData(sampleData.getBothData4Log());
                    result += " ";
                    result += predictionModel.predictBothData(sampleData.getBothData5Log());
                }
                else if (usePort == SensorToUse.port1)
                {
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data1Log());
                    result += " ";
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data2Log());
                    result += " ";
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data3Log());
                    result += " ";
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data4Log());
                    result += " ";
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data5Log());
                }
                else if (usePort == SensorToUse.port2)
                {
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data1Log());
                    result += " ";
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data2Log());
                    result += " ";
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data3Log());
                    result += " ";
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data4Log());
                    result += " ";
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data5Log());
                }
                else // if (usePort == SensorToUse.port1or2)
                {
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data1Log());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data2Log());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data3Log());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data4Log());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data5Log());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data1Log());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data2Log());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data3Log());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data4Log());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data5Log());
                }
                result += "\r\n";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] testTrainingResult() " + ex.Message);
            }
            return (result);
        }

        private String testTrainingResult(SensorToUse usePort, ref IPredictionModel predictionModel)
        {
            String result = "";
            try
            {
                TestSampleData sampleData = new TestSampleData();
                if (usePort == SensorToUse.port1and2)
                {
                    result += predictionModel.predictBothData(sampleData.getBothData1());
                    result += " ";
                    result += predictionModel.predictBothData(sampleData.getBothData2());
                    result += " ";
                    result += predictionModel.predictBothData(sampleData.getBothData3());
                    result += " ";
                    result += predictionModel.predictBothData(sampleData.getBothData4());
                    result += " ";
                    result += predictionModel.predictBothData(sampleData.getBothData5());
                }
                else if (usePort == SensorToUse.port1)
                {
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data1());
                    result += " ";
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data2());
                    result += " ";
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data3());
                    result += " ";
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data4());
                    result += " ";
                    result += predictionModel.predictSingle1Data(sampleData.getOdor1Data5());
                }
                else if (usePort == SensorToUse.port2)
                {
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data1());
                    result += " ";
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data2());
                    result += " ";
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data3());
                    result += " ";
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data4());
                    result += " ";
                    result += predictionModel.predictSingle2Data(sampleData.getOdor2Data5());
                }
                else // if (usePort == SensorToUse.port1or2)
                {
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data1());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data2());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data3());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data4());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr1Data5());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data1());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data2());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data3());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data4());
                    result += " ";
                    result += predictionModel.predictOrData(sampleData.getOdorOr2Data5());
                }
                result += "\r\n";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] testTrainingResult() " + ex.Message);
            }
            return (result);
        }
    }
}
