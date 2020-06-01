using System;
using Autofac;
using MiniWeb.Core.Abstractions;

namespace MiniWeb.Server.Autofac
{
    internal class AutofacDependencyProvider : IDependencyProvider
    {
        private readonly IContainer _container;

        public AutofacDependencyProvider(IContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public TService Get<TService>()
        {
            return _container.Resolve<TService>();
        }

        public object Get(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public TService GetNamed<TService>(string serviceName)
        {
            return _container.ResolveNamed<TService>(serviceName);
        }
    }
}
