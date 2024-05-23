using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
