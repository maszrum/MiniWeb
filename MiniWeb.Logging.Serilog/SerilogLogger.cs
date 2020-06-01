using System;
using MiniWeb.Core.Abstractions;
using Serilog;

namespace MiniWeb.Logging.Serilog
{
    internal class SerilogLogger
    {
        private readonly ILogger _logger;

        public SerilogLogger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void LogException(IWebRequest request, Exception exception)
        {
            _logger.Error(
                exception,
                "Exception thrown while processing request {RequestId}", 
                request.Id.ToString("N"));
        }
    }
}
