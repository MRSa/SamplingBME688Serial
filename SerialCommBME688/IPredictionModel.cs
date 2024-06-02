namespace SamplingBME688Serial
{
    public interface IPredictionModel
    {
        string getMethodName();
        bool savePredictionModel(string outputFileName);
        string predictBothData(OdorBothData targetData);
        string predictOrData(OdorOrData targetData);
        string predictSingle1Data(OdorData targetData);
        string predictSingle2Data(OdorData targetData);

        string predictBothData(SmellBothData targetData);
        string predictOrData(SmellOrData targetData);
        string predictSingle1Data(SmellData targetData);
        string predictSingle2Data(SmellData targetData);
    }
}
