using System.Net;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server.Responses;

namespace MiniWeb.Json
{
    public abstract class JsonEndpoint<T> : IWebEndpoint where T : class
    {
        public abstract IWebResponse Process(T input, IWebRequest request);

        public IWebResponse Process(IWebRequest request)
        {
            if (!IsContentTypeJson(request))
            {
                return UnsupportedMediaType();
            }

            var jsonRequest = request.ToJson();

            try
            {
                var input = jsonRequest.DeserializeObject<T>();
                return Process(input, request);
            }
#pragma warning disable CA1031
            catch
            {
                return UnsupportedMediaType();
            }
#pragma warning restore CA1031
        }

        private static bool IsContentTypeJson(IWebRequest request)
        {
            var contentType = request.GetHeader(HttpRequestHeader.ContentType);
            return contentType == "application/json";
        }

        public IWebResponse UnsupportedMediaType()
        {
            return new PlainTextResponse(
                "invalid input data", HttpStatusCode.UnsupportedMediaType);
        }
    }
}
