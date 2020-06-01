using System;
using System.Net;
using System.Text;

namespace MiniWeb.Server.Responses
{
    public class MethodNotAllowedResponse : BaseResponse
    {
        private readonly string _content = null;
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.MethodNotAllowed;

        public MethodNotAllowedResponse()
        {
        }

        public MethodNotAllowedResponse(string content)
        {
            _content = content ?? throw new ArgumentNullException(nameof(content));
        }

        protected override void Rewrite(HttpListenerResponse response)
        {
            if (_content != null)
            {
                response.ContentType = "text/plain";

                var buffer = Encoding.UTF8.GetBytes(_content);
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
