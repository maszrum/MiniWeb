using System;
using System.Net;
using System.Text;

namespace MiniWeb.Server.Responses
{
    public class UnauthorizedResponse : BaseResponse
    {
        private readonly string _content;
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Unauthorized;

        public UnauthorizedResponse()
        {
        }

        public UnauthorizedResponse(string content)
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
