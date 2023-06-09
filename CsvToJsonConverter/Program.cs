using CsvToJsonConverter.Process.Core;
using Serilog;
using Serilog.Events;

namespace CsvToJsonConverter
{
    public class Program
    {
        

        public static void Main(string[] args)
        {            
            
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                         .Enrich.FromLogContext()
                         .WriteTo.File("C:\\Users\\amohanja\\WorkRelated\\Project\\Worker Service Logs\\LogFile.txt")
                         .CreateLogger();

            Environment.SetEnvironmentVariable("CSV_FILE_PATH", "C:\\Users\\amohanja\\WorkRelated\\Project");
            Environment.SetEnvironmentVariable("LOG_FILE_PATH", "C:\\Users\\amohanja\\WorkRelated\\Project\\Worker Service Logs");
            Environment.SetEnvironmentVariable("JSON_FILE_NAME", "Sources.json");

            try
            {
                Log.Write(LogEventLevel.Information, "Service started");
                
                IHost host = Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                    //services.AddSingleton<IChildCancellationTokens, ChildCancellationTokens>();
                })
                .UseSerilog()
                .Build();

                host.Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error occured");
                return;
            }
            finally
            {
                Log.CloseAndFlush();                
            }
        }
    }
}