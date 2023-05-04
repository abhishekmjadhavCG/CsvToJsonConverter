using CsvToJsonConverter.Process.Core.Entity;
using Microsoft.Extensions.Logging;
using CsvToJsonConverter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CsvToJsonConverter.Process.Core
{
    public class CSVProcessor : ICSVProcessor
    {
        #region Declarations
        
        string[] headers = new string[] { };
        List<Customer> customers = new List<Customer>();
        List<string> jsonCustomers = new List<string>();
        private readonly ILogger<CSVProcessor> _logger;
        //private readonly IChildCancellationTokens _childCancellationTokens;
        CancellationToken token = new CancellationToken();
        private readonly IConfiguration _configuration;
        private string destinationFilePath = string.Empty;

        #endregion

        public CSVProcessor() 
        {                        
            var filePath = Environment.GetEnvironmentVariable("CSV_FILE_PATH");
            var destinationFileName = Environment.GetEnvironmentVariable("JSON_FILE_NAME");            
            destinationFilePath = Path.Combine(filePath, destinationFileName);
        }


        #region ProcessFile
        
        public async Task ProcessFile(string filePath)
        {
            await ReadFileAsync(filePath, token);
        }

        #endregion

        #region ReadFile
        public async Task ReadFileAsync(string filePath, CancellationToken token)
        {

            string[] data = new string[] { };

            StreamReader reader = new StreamReader(filePath);
            StringBuilder fileContents = new();

            //read line by line				
            try
            {
                var counter = 0;

                Stopwatch watch = new Stopwatch();
                
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    Log.Error("Operation cancelled. Stopping the service");
                }
                
                watch.Start();
                
                Log.Information($"File read started..");

                while (!reader.EndOfStream)
                {
                    var singleLine = await reader.ReadLineAsync();
                    
                    if (string.IsNullOrWhiteSpace(singleLine))
                    {
                        token.ThrowIfCancellationRequested();
                        Log.Error("Operation cancelled. Stopping the service");
                    }

                    if (counter == 0)
                    {
                        headers = singleLine.Split('|');
                        counter++;
                        continue;
                    }
                    else
                    {
                        data = singleLine.Split('|');
                    }

                    var jsonCustomer =  await Task.Run(() => TransformData(data, token));
                    fileContents.AppendLine(jsonCustomer);
                    
                    counter++;
                }

                reader.Close();

                Log.Information($"Time taken for reading csv file - {watch.Elapsed}");

                watch.Stop();
                
                //Writing to destination file
                await WriteAllLineAsync(fileContents, token);
                                
                counter = 0;                
            }
            catch (Exception ex)
            {
                reader.Close();
                Log.Error(ex.StackTrace, "Error occured in  ReadFileAsync method.");
            }
            finally
            {
                reader.Close();
                reader.Dispose();
            }
        }

        #endregion

        #region TransformData

        private string TransformData(string[] data, CancellationToken token)
        {
            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    Log.Error("Operation cancelled. Stopping the service");
                }
                
                var customer = PopulateCustomerWithData(data, token);
                
                return ConvertEntityToJson(customer, token);                                
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, "Error occured in TransformData method");                                
            }

            return string.Empty;
        }

        #endregion

        #region PopulateCustomerWithData

        private Customer PopulateCustomerWithData(string[] data, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
                Log.Error("Operation cancelled. Stopping the service");
            }
            Customer customer = new Customer();
            customer.CUSTOMER_UNIQUE_ID = data[0];
            customer.CUSTOMER_SOURCE_REF_ID = data[1];
            customer.CUSTOMER_INTERMEDIARY_REF_ID = data[2];
            customer.PERSON_TITLE = data[3];
            customer.FIRST_NAME = data[4];
            customer.LAST_NAME = data[5];
            customer.SUFFIX = data[6];
            customer.CUSTOMER_NAME = data[7];
            customer.CUSTOMER_NAME_1 = data[8];
            customer.CUSTOMER_NAME_2 = data[9];
            customer.CUSTOMER_NAME_3 = data[10];
            customer.CUSTOMER_NAME_4 = data[11];
            customer.REGISTERED_NUMBER = data[12];
            customer.DATE_OF_BIRTH = data[13];
            customer.ADDRESS = data[14];
            customer.ZONE = data[15];
            customer.POSTAL_CODE = data[16];
            customer.COUNTRY_OF_RESIDENCE = data[17];
            customer.COUNTRY_OF_ORIGIN = data[18];
            customer.NATIONALITY_CODE = data[19];
            customer.GENDER_CODE = data[20];
            customer.EMPLOYEE_FLAG = data[21];
            customer.OCCUPATION = data[22];
            customer.ACQUISITION_DATE = data[23];
            customer.CANCELLED_DATE = data[24];
            customer.CUSTOMER_TYPE_CODE = data[25];
            customer.LAST_UPDATED_TIMESTAMP = data[26];
            customer.COMPLEX_STRUCTURE = data[27];
            customer.BLACK_LISTED_FLAG = data[28];
            customer.COMPANY_CODE = data[29];
            customer.ORG_UNIT_CODE = data[30];
            customer.DOMAIN = data[31];
            customer.COMMENTS = data[32];
            customer.PEP_FLAG = data[33];
            customer.VERSION = data[34];
            customer.UPDATED_DATE = data[35];
            customer.CREATION_DATE = data[36];
            customer.SOURCE_SYSTEM = data[37];
            customer.SENSITIVE_CUSTOMER_FLAG = data[38];
            return customer;
        }

        #endregion

        #region ConvertEntityToJson
        private string ConvertEntityToJson(Customer customer, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            return JsonConvert.SerializeObject(customer);             
        }

        #endregion

        #region Write File

        private async Task WriteAllLineAsync(StringBuilder stringBuilder, CancellationToken token)
        {
            Stopwatch writeWatcher = new Stopwatch();            
            
            writeWatcher.Start();
           
            Log.Information($"File write started..");

            try
            {
                await File.AppendAllTextAsync(destinationFilePath, stringBuilder.ToString(), token);
            }
            catch(Exception ex)
            {
                Log.Error(ex.StackTrace, "Error occured while writing file.");
            }
            
            Log.Information($"Time taken for file write operation - {writeWatcher.Elapsed}.");
            
            writeWatcher.Stop();

            Log.Information($"File write completed successfully");
            
        }

        public async Task WriteFileAsync(string data, CancellationToken token)
        {
            //large buffer size
            //StreamWriter streamWriter = new StreamWriter(destinationFilePath, true, Encoding.UTF8, 65536);
            StreamWriter streamWriter = new StreamWriter(destinationFilePath, true);
            try
            {                
                await streamWriter.WriteLineAsync(data);
            }
            catch (Exception ex) 
            {                               
                Log.Error(ex.StackTrace, "Error occured while writing file.");
            }
            finally
            { 
                streamWriter.Close();
                await streamWriter.DisposeAsync();
            }
        }

        #endregion
    }
}
