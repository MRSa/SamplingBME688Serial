using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    public class Bme688DataSummary
    {
        // BME688の受信データをサマライズしたデータを格納するクラス
        public string category { get; private set; }
        public int sensorId { get; private set; }
        public int sampleCount { get; private set; }
        public int validCount { get; private set; }
        public double temperature_max { get; private set; }
        public double temperature_min { get; private set; }
        public double humidity_max { get; private set; }
        public double humidity_min { get; private set; }
        public double pressure_max { get; private set; }
        public double pressure_min { get; private set; }
        public double gas_registance_max { get; private set; }
        public double gas_registance_min { get; private set; }
        public double gas_registance_log_max { get; private set; }
        public double gas_registance_log_min { get; private set; }

        public Bme688DataSummary(string category,
                                 int sensorId,
                                 int sampleCount,
                                 int validCount,
                                 double temperature_max,
                                 double temperature_min,
                                 double humidity_max,
                                 double humidity_min,
                                 double pressure_max,
                                 double pressure_min,
                                 double gas_registance_max,
                                 double gas_registance_min, 
                                 double gas_registance_log_max,
                                 double gas_registance_log_min)
        {
            this.category = category;
            this.sensorId = sensorId;
            this.sampleCount = sampleCount;
            this.validCount = validCount;
            this.temperature_max = temperature_max;
            this.temperature_min = temperature_min;
            this.humidity_max = humidity_max;
            this.humidity_min = humidity_min;
            this.pressure_max = pressure_max;
            this.pressure_min = pressure_min;
            this.gas_registance_max = gas_registance_max;
            this.gas_registance_min = gas_registance_min;
            this.gas_registance_log_max = gas_registance_log_max;
            this.gas_registance_log_min = gas_registance_log_min;
        }
    }
}
