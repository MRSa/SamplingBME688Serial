namespace SamplingBME688Serial
{
    class CheckResult
    {
        public string resultLabel { get; private set; }
        public DateTime sampleTime { get; private set; }

        public CheckResult(string resultLabel)
        {
            this.resultLabel = resultLabel;
            sampleTime = DateTime.Now;
        }
    }
}
