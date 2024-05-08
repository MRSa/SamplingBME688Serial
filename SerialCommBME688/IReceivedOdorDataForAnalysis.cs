using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    interface IReceivedOdorDataForAnalysis
    {
        void receivedOdorDataForAnalysis(OdorOrData receivedData);
    }
}
