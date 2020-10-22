using System.Net;

namespace MiniWeb.Server.Responses
{
    public sealed class NoContentResponse : BaseResponse
    {
        public override HttpStatusCode StatusCode { get; set; }

        public NoContentResponse() : this(HttpStatusCode.NoContent)
        {
        }

        public NoContentResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        protected override void Rewrite(HttpListenerResponse response)
        {
        }
    }
}
