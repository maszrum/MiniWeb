using System;
using MiniWeb.Core.Abstractions;

namespace MiniWeb.Server
{
    public class Interceptor : IInterceptor
    {
        public virtual IWebResponse BeforeProcessing(IWebRequest request, Type endpointType)
        {
            return null;
        }

        public virtual IWebResponse AfterProcessing(IWebRequest request, IWebResponse response)
        {
            return null;
        }
    }
}
