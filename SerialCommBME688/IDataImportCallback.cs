namespace SamplingBME688Serial
{
    public interface IDataImportCallback
    {
        public void dataImportFinished(bool isSuccess, String message);
        public void dataImportProgress(int lineNumber, int totalLines);
    }
}
