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
    }

    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[]? Distances;
    }
}
