namespace SamplingBME688Serial
{
    public interface ICreateModelResult
    {
        public void createModelFinished(bool isSuccess, SensorToUse modelType, IPredictionModel? predictionModel, string message);

    }
}
