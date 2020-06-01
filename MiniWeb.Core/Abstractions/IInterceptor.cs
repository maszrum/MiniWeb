using System;

namespace MiniWeb.Core.Abstractions
{
    public interface IInterceptor
    {
        IWebResponse BeforeProcessing(IWebRequest request, Type endpointType);
        IWebResponse AfterProcessing(IWebRequest request, IWebResponse response);
    }
}
