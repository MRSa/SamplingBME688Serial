using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    public enum SensorToUse
    {
        None,
        port1and2,
        port1,
        port2,
        port1or2
    }

    interface ICreatePredictionModel
    {
        bool executeTraining(SensorToUse usePort, String? outputFileName, ref IDataHolder? port1, ref IDataHolder? port2, bool isLogData);
        bool loadPredictionModel(SensorToUse sensorToUse, String inputFileName);
    }
}
