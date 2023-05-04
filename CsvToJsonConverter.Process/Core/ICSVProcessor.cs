using CsvToJsonConverter.Process.Core.Entity;

namespace CsvToJsonConverter.Process.Core
{
    public interface ICSVProcessor
    {        
        public Task ProcessFile(string filePath);
        public Task ReadFileAsync(string filePath, CancellationToken token);
        public Task WriteFileAsync(string data, CancellationToken token);        
    }
}