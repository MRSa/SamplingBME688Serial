namespace SerialCommBME688
{
    internal class Bme688Data
    {
        // BME688から受信したデータを格納するクラス。
        public double temperature { get; private set; }
        public double humidity { get; private set; }
        public double pressure { get; private set; }
        public double gas_registance { get; private set; }
        public double gas_registance_log { get; private set; }
        public double gas_registance_diff { get; private set; }
        public bool isValid { get; private set; } = false;

        public void setBme688Data(double temperature, double humidity, double pressure, double gas_registance, double gas_registance_log, double gas_registance_diff)
        {
            this.temperature = temperature;
            this.humidity = humidity;
            this.pressure = pressure;
            this.gas_registance = gas_registance;
            this.gas_registance_log = gas_registance_log;
            this.gas_registance_diff = gas_registance_diff;

            isValid = true;
        }
    }
}
