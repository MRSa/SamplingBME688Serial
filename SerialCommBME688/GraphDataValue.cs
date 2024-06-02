namespace SamplingBME688Serial
{
    public class GraphDataValue
    {

        public double pressure { get; private set; }
        public double temperature { get; private set; }
        public double humidity { get; private set; }

        public double gas_registance { get; private set; }
        public double gas_registance_log { get; private set; }

        public GraphDataValue(double gas_registance, double gas_registance_log, double pressure, double temperature, double humidity)
        {
            this.gas_registance = gas_registance;
            this.gas_registance_log = gas_registance_log;
            this.pressure = pressure;
            this.temperature = temperature;
            this.humidity = humidity;
        }
    }
}
