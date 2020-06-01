using System;

namespace MiniWeb.Core.Abstractions
{
    public interface IRegisteredEndpoint
    {
        string Method { get; }
        string Endpoint { get; }
        Type HandlerType { get; }
    }
}
