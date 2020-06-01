using System;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server;
using MiniWeb.Server.Common;
using Serilog;

namespace MiniWeb.Logging.Serilog
{
    internal class SerilogInterceptor : Interceptor
    {
        private const string BeginTimestampKey = "BeginTimestamp";

        private readonly ILogger _logger;

        public SerilogInterceptor(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override IWebResponse BeforeProcessing(IWebRequest request, Type endpointType)
        {
            var method = request.Method.ToString().ToUpper();

            request.SetData(BeginTimestampKey, DateTime.UtcNow.Ticks);

            _logger.Information(
                "Received request from {Address}{NewLine}Id: {RequestId}{NewLine}{RequestMethod} {RequestRoute}{NewLine}Endpoint: {Endpoint}",
                request.RemoteAddress,
                Environment.NewLine,
                request.Id.ToString("N"),
                Environment.NewLine,
                method,
                request.RawUrl,
                Environment.NewLine,
                endpointType.Name);

            return null;
        }

        public override IWebResponse AfterProcessing(IWebRequest request, IWebResponse response)
        {
            var ticks = request.GetData<long>(BeginTimestampKey);
            var timeSpan = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - ticks);
            
            if (request.TryGetException(out var exception))
            {
                _logger.Warning(
                    exception,
                    "Response for {RequestId}{NewLine}Status code: {StatusCode}{NewLine}Processing time: {TimeSpan}",
                    request.Id,
                    Environment.NewLine,
                    response.StatusCode,
                    Environment.NewLine,
                    timeSpan);
            }
            else
            {
                _logger.Information(
                    "Response for {RequestId}{NewLine}Status code: {StatusCode}{NewLine}Processing time: {TimeSpan}",
                    request.Id,
                    Environment.NewLine,
                    response.StatusCode,
                    Environment.NewLine,
                    timeSpan);
            }

            return null;
        }
    }
}
