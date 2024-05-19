
namespace SamplingBME688Serial
{
    interface ILoadPredictionModel
    {
        bool loadPredictionModel(SensorToUse sensorToUse, string inputFileName);
    }
}
