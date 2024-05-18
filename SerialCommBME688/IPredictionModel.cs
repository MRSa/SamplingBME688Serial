using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    public interface IPredictionModel
    {
        bool savePredictionModel(SensorToUse sensorToUse, String outputFileName);

        String predictBothData(OdorBothData targetData);
        String predictOrData(OdorOrData targetData);
        String predictSingle1Data(OdorData targetData);
        String predictSingle2Data(OdorData targetData);
    }
}
