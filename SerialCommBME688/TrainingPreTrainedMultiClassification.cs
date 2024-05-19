using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    class TrainingPreTrainedMultiClassification : ILoadPredictionModel, IPredictionModel
    {
        public bool loadPredictionModel(SensorToUse sensorToUse, String inputFileName)
        {
            return (false);
        }

        public bool savePredictionModel(String outputFileName)
        {
            // ----- ロードしたモデルの場合は、保存ができません
            return (false);
        }

        public String predictBothData(OdorBothData targetData)
        {
            return ("");
        }

        public String predictOrData(OdorOrData targetData)
        {
            return ("");
        }

        public String predictSingle1Data(OdorData targetData)
        {
            return ("");
        }
        public String predictSingle2Data(OdorData targetData)
        {
            return ("");
        }
    
    }
}
