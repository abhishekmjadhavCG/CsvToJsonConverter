using CsvToJsonConverter;
using CsvToJsonConverter.Process.Core;
using System.Threading;
using Serilog;

namespace CsvToJsonConverter
{
    public class Worker : BackgroundService
    {
        //private string soucefileName = "Test2.csv"; //Records - three million, sixty-four thousand, seven hundred forty-six        
        private string soucefileName = "262_SIA_W_20230423_1500.nrk";        
        private readonly IConfiguration _configuration;
        private readonly ICSVProcessor _csvProcessor;
        private static string csvPath = string.Empty;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private CancellationToken cancellationToken = new CancellationToken();
        private CSVProcessor csvProcessor;

        public Worker(IConfiguration configuration)
        {            
            _configuration = configuration;
            cancellationToken = tokenSource.Token;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {            
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            csvPath = string.Empty;
            Log.Warning("The service is stopping now..");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var isFileValidated = await Task.Run(() => ValidatePathWithFile());

            if (!isFileValidated)
            {
                throw new TaskCanceledException();
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                var isFileExists = File.Exists(csvPath);
                if (!isFileExists)
                {
                    Log.Fatal("File not found at the specified path..");
                }
                else
                {
                    csvProcessor = new CSVProcessor();
                    await csvProcessor.ProcessFile(csvPath);
                    break;
                }

                Log.Information("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromDays(1), cancellationToken);
            }
        }

        private bool ValidatePathWithFile()
        {
            Log.Information("Reading the file path..");

            csvPath = _configuration.GetSection("CSV_FILE_PATH").Value;
            csvPath = Path.Combine(csvPath, soucefileName);

            
            if (string.IsNullOrWhiteSpace(csvPath) || 
                string.IsNullOrWhiteSpace(soucefileName))
            {
                Log.Error("Invalid file or file path..");                                                               
                return false;
            }

            if (!File.Exists(csvPath))
            {
                Log.Error("File does not exist at the path..");
                return false;
            }

            return true;
        }
    }
}