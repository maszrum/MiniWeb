using System;
using System.Net;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server.Responses;

namespace MiniWeb.Server.Endpoints
{
    internal class InternalServerErrorEndpoint : IWebEndpoint
    {
        public Exception Exception { get; set; }

        public InternalServerErrorEndpoint()
        {
        }

        public InternalServerErrorEndpoint(Exception exception)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public IWebResponse Process(IWebRequest request)
        {
            var exceptionMessage = string.Empty;
            if (Exception != null)
            {
                exceptionMessage = $": {Exception.GetType().Name} ({Exception.Message})";
            }

            return new PlainTextResponse(
                $"internal server error" + exceptionMessage, HttpStatusCode.InternalServerError);
        }
    }
}
