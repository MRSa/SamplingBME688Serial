using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    public interface ICreateModelResult
    {
        public void createModelFinished(bool isSuccess, SensorToUse modelType, IPredictionModel? predictionModel, string message);

    }
}
