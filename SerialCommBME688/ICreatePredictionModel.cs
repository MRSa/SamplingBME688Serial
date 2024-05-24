
namespace SamplingBME688Serial
{
    public enum SensorToUse
    {
        None,
        port1and2,
        port1,
        port2,
        port1or2
    }

    interface ICreatePredictionModel
    {
        bool executeTraining(SensorToUse usePort, string? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData, bool withPresTempHum);
    }
}
