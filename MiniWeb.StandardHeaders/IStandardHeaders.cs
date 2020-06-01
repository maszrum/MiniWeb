using System;
using System.Collections.Generic;

namespace MiniWeb.StandardHeaders
{
    internal interface IStandardHeaders : IEnumerable<KeyValuePair<string, string>>
    {
        bool ContainsHeader(string headerName);
        void RegisterFactory(string headerName, Func<string> factory);
        void RegisterValue(string headerName, string value);
    }
}