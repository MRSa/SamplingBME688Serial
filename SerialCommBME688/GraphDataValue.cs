
namespace SamplingBME688Serial
{
    internal class GraphDataValue
    {
        public double gas_registance { get; private set; }
        public double gas_registance_log { get; private set; }

        public GraphDataValue(double gas_registance, double gas_registance_log)
        {
            this.gas_registance = gas_registance;
            this.gas_registance_log = gas_registance_log;
        }
    }
}
