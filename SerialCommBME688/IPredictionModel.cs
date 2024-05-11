using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    public interface IPredictionModel
    {
        String predictBothData(OdorBothData targetData);
        String predictOrData(OdorOrData targetData);
        String predictSingleData(OdorData targetData);
    }
}
