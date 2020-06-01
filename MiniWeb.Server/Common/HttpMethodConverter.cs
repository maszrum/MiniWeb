using System;
using MiniWeb.Core;

namespace MiniWeb.Server.Common
{
    public static class HttpMethodConverter
    {
        public static HttpMethod Convert(string input)
        {
            var transformed = $"{input[0]}{input.Substring(1).ToLower()}";

            return (HttpMethod)Enum.Parse(typeof(HttpMethod), transformed);
        }
    }
}
