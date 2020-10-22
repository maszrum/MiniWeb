using System;
using System.Net;
using System.Text;

namespace MiniWeb.Server.Responses
{
    public sealed class PlainTextResponse : BaseResponse
    {
        public override HttpStatusCode StatusCode { get; set; }
        public string Text { get; set; }

        public PlainTextResponse(string text) : this(text, HttpStatusCode.OK)
        {
        }

        public PlainTextResponse(string text, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        protected override void Rewrite(HttpListenerResponse response)
        {
            response.ContentType = "text/plain";

            var buffer = Encoding.UTF8.GetBytes(Text);
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}
