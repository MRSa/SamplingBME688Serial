namespace SamplingBME688Serial
{
    public interface ILoadDataFromDatabase
    {
        void LoadDataFromDatabase(List<LoadSensorDataInformation> categoryToLoad);
    }
}
