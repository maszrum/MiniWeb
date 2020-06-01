using System;
using System.Net;
using System.Text;

namespace MiniWeb.Server.Responses
{
    public class NotFoundResponse : BaseResponse
    {
        private readonly string _content;
        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;

        public NotFoundResponse() 
        {
        }

        public NotFoundResponse(string content)
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
