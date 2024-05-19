
namespace SamplingBME688Serial
{
    public interface IPredictionModel
    {
        string getMethodName();
        bool savePredictionModel(String outputFileName);
        String predictBothData(OdorBothData targetData);
        String predictOrData(OdorOrData targetData);
        String predictSingle1Data(OdorData targetData);
        String predictSingle2Data(OdorData targetData);
    }
}
