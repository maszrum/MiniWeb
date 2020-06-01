using System;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server;

namespace MiniWeb.StandardHeaders
{
    internal class StandardHeadersInterceptor : Interceptor
    {
        private readonly IStandardHeaders _headers;

        public StandardHeadersInterceptor(IStandardHeaders headers)
        {
            _headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }

        public override IWebResponse AfterProcessing(IWebRequest request, IWebResponse response)
        {
            foreach (var (header, value) in _headers)
            {
                response.SetHeader(header, value);
            }

            return response;
        }
    }
}
