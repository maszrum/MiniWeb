using System;
using System.IO;
using System.Net;
using MiniWeb.Server.Responses;
using Newtonsoft.Json;

namespace MiniWeb.Json
{
    public sealed class JsonResponse : BaseResponse
    {
        public override HttpStatusCode StatusCode { get; set; }
        public object Content { get; set; }

        public JsonResponse(object content) : this(content, HttpStatusCode.OK)
        {
        }

        public JsonResponse(object content, HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        protected override void Rewrite(HttpListenerResponse response)
        {
            response.ContentType = "application/json";

            var serializer = new JsonSerializer();

            using var writer = new StreamWriter(response.OutputStream);
            using var jsonWriter = new JsonTextWriter(writer);
            serializer.Serialize(jsonWriter, Content);
        }
    }
}
