using System;
using MiniWeb.Core.Abstractions;

namespace MiniWeb.Server.Common
{
    public static class WebRequestExtension
    {
        private const string ExceptionKey = "Exception";

        public static bool TryGetException(this IWebRequest request, out Exception exception)
        {
            return request.TryGetData(ExceptionKey, out exception);
        }

        public static void SetException(this IWebRequest request, Exception exception)
        {
            request.SetData(ExceptionKey, exception);
        }
    }
}
