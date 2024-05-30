namespace SamplingBME688Serial
{ 
    public class LoadSensorDataInformation
    {
        public string dataCategory { get; private set; }
        public int sensorId { get; private set; }
        public int startFrom { get; private set; }
        public int dataCount { get; private set; }
        public LoadSensorDataInformation(string dataCategory, int sensorId, int startFrom, int dataCount)
        {
            this.dataCategory = dataCategory;
            this.sensorId = sensorId;
            this.startFrom = startFrom;
            this.dataCount = dataCount;
        }
    }
}
