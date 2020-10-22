using System;
using System.IO;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server.Requests;
using Newtonsoft.Json;

namespace MiniWeb.Json
{
    public class JsonRequest : BaseRequest
    {
        public JsonRequest(BaseRequest request) : base(request)
        {
        }

        public T DeserializeObject<T>()
        {
            var serializer = new JsonSerializer();
            T result;

            using (var reader = new StreamReader(Request.InputStream))
            {
                using var jsonReader = new JsonTextReader(reader);
                result = serializer.Deserialize<T>(jsonReader);
            }

            return result;
        }
    }

    public static class JsonRequestExtension
    {
        public static JsonRequest ToJson(this IWebRequest request)
        {
            if (request is BaseRequest requestBase)
            {
                return new JsonRequest(requestBase);
            }

            throw new InvalidCastException(
                $"cannot cast {nameof(IWebRequest)} to {nameof(JsonRequest)}");
        }
    }
}
