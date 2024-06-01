namespace SamplingBME688Serial
{
    public interface ILoadDataFromDatabase
    {
        void LoadDataFromDatabase(ref List<LoadSensorDataInformation> categoryToLoad);
    }
}
