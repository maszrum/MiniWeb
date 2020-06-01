using System.Net;

namespace MiniWeb.Core.Abstractions
{
    public interface IWebResponse
    {
        HttpStatusCode StatusCode { get; }

        void ApplyTo(HttpListenerResponse response);
        
        void SetHeader(string header, string value);
        void SetHeader(HttpResponseHeader header, string value);
    }
}
