using Microsoft.ML.Data;

namespace SamplingBME688Serial
{
    public class OdorData
    {
        [LoadColumn(0)]
        public float sequence0Value;

        [LoadColumn(1)]
        public float sequence1Value;

        [LoadColumn(2)]
        public float sequence2Value;

        [LoadColumn(3)]
        public float sequence3Value;

        [LoadColumn(4)]
        public float sequence4Value;

        [LoadColumn(5)]
        public float sequence5Value;

        [LoadColumn(6)]
        public float sequence6Value;

        [LoadColumn(7)]
        public float sequence7Value;

        [LoadColumn(8)]
        public float sequence8Value;

        [LoadColumn(9)]
        public float sequence9Value;

        [LoadColumn(10)]
        public string? dataLabel;

        public OdorData(OdorData data)
        {
            this.sequence0Value = data.sequence0Value;
            this.sequence1Value = data.sequence1Value;
            this.sequence2Value = data.sequence2Value;
            this.sequence3Value = data.sequence3Value;
            this.sequence4Value = data.sequence4Value;
            this.sequence5Value = data.sequence5Value;
            this.sequence6Value = data.sequence6Value;
            this.sequence7Value = data.sequence7Value;
            this.sequence8Value = data.sequence8Value;
            this.sequence9Value = data.sequence9Value;
            this.dataLabel = data.dataLabel;
        }

        public OdorData(OdorOrData data)
        {
            this.sequence0Value = data.sequence0Value;
            this.sequence1Value = data.sequence1Value;
            this.sequence2Value = data.sequence2Value;
            this.sequence3Value = data.sequence3Value;
            this.sequence4Value = data.sequence4Value;
            this.sequence5Value = data.sequence5Value;
            this.sequence6Value = data.sequence6Value;
            this.sequence7Value = data.sequence7Value;
            this.sequence8Value = data.sequence8Value;
            this.sequence9Value = data.sequence9Value;
            this.dataLabel = data.dataLabel;
        }

        public OdorData(SmellOrData data)
        {
            this.sequence0Value = data.sequence0Value;
            this.sequence1Value = data.sequence1Value;
            this.sequence2Value = data.sequence2Value;
            this.sequence3Value = data.sequence3Value;
            this.sequence4Value = data.sequence4Value;
            this.sequence5Value = data.sequence5Value;
            this.sequence6Value = data.sequence6Value;
            this.sequence7Value = data.sequence7Value;
            this.sequence8Value = data.sequence8Value;
            this.sequence9Value = data.sequence9Value;
            this.dataLabel = data.dataLabel;
        }

        public OdorData() {  }
    }
}
