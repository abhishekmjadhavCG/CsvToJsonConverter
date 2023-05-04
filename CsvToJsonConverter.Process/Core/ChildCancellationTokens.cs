using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToJsonConverter.Process.Core
{
    public sealed class ChildCancellationTokens : IChildCancellationTokens
    {
        private readonly ILogger _logger;

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        public ChildCancellationTokens(ILogger<ChildCancellationTokens> logger) 
        {
            _logger = logger;
        }

        public CancellationToken GenerateCancellationToken()
        {
            _logger.LogInformation("Generating cancellation token for child classes..");
            return tokenSource.Token;            
        }

        public void CancelOperation()
        {
            _logger.LogInformation("Cancelling the operation for file processing..");
            tokenSource.Cancel();
        }
    }
}
