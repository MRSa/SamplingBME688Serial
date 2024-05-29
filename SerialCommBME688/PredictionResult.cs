using Microsoft.ML.Data;

namespace SamplingBME688Serial
{
    public class PredictionResult
    {
        [ColumnName("PredictedLabel")]
        public string? dataLabel;

        [ColumnName("Score")]
        public float[]? Distances;
    }
}
