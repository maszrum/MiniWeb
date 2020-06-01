using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace MiniWeb.Core.Abstractions
{
    public interface IWebRequest : IRequestData
    {
        Guid Id { get; }
        Stream InputStream { get; }
        long ContentLength { get; }
        IReadOnlyList<string> Route { get; }
        NameValueCollection QueryString { get; }
        HttpMethod Method { get; }
        IPAddress RemoteAddress { get; }
        string RawUrl { get; }

        string GetHeader(string header);
        string GetHeader(HttpRequestHeader header);
    }
}
