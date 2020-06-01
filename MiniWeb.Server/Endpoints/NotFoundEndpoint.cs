using MiniWeb.Core.Abstractions;
using MiniWeb.Server.Responses;

namespace MiniWeb.Server.Endpoints
{
    internal class NotFoundEndpoint : IWebEndpoint
    {
        public IWebResponse Process(IWebRequest request)
        {
            return new NotFoundResponse();
        }
    }
}
