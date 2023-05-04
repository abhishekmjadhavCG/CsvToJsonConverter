namespace CsvToJsonConverter.Process.Core
{
    public interface IChildCancellationTokens
    {
        CancellationToken GenerateCancellationToken();

        void CancelOperation();
    }
}