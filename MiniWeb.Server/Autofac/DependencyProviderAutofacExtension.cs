using Autofac;
using MiniWeb.Core.Abstractions;

namespace MiniWeb.Server.Autofac
{
    public static class DependencyProviderAutofacExtension
    {
        public static IDependencyProvider ToDependencyProvider(this IContainer container)
        {
            return new AutofacDependencyProvider(container);
        }
    }
}
