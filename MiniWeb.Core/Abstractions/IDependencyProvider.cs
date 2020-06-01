using System;

namespace MiniWeb.Core.Abstractions
{
    public interface IDependencyProvider
    {
        TService Get<TService>();
        TService GetNamed<TService>(string serviceName);
        object Get(Type serviceType);
    }
}
