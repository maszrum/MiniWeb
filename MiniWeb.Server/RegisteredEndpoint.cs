using System;
using MiniWeb.Core;
using MiniWeb.Core.Abstractions;

namespace MiniWeb.Server
{
    public class RegisteredEndpoint : IRegisteredEndpoint
    {
        private bool? _parameterlessConstructor;

        public string Method { get; }
        public string Endpoint { get; }
        public Type HandlerType { get; }

        private RegisteredEndpoint(string method, string endpoint, Type handlerType)
        {
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            Method = method;
            Endpoint = endpoint;
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
        }

        public IWebEndpoint Instantiate(params object[] args)
        {
            if (!_parameterlessConstructor.HasValue)
            {
                var result = InstantiateForFirstTime(out var parameterless, args);
                _parameterlessConstructor = parameterless;
                return result;
            }

            if (_parameterlessConstructor.Value)
            {
                return InstantiateParameterless();
            }
            else
            {
                return InstantiateWithParameters(args);
            }
        }

        private IWebEndpoint InstantiateForFirstTime(out bool parameterless, params object[] args)
        {
            var result = TryInstantiateWithParameters(args);
            if (result == null)
            {
                result = TryInstantiateParameterless();
                if (result == null)
                {
                    throw new MissingMethodException(
                        $"cannot find matching constructor for endpoint: {HandlerType.Name}");
                }

                parameterless = true;
            }
            else
            {
                parameterless = false;
            }
            return result;
        }

        private IWebEndpoint InstantiateParameterless()
        {
            return (IWebEndpoint)Activator.CreateInstance(HandlerType);
        }

        private IWebEndpoint TryInstantiateParameterless()
        {
            try
            {
                return InstantiateParameterless();
            }
#pragma warning disable CA1031
            catch (MissingMethodException)
            {
                return null;
            }
#pragma warning restore CA1031
        }

        private IWebEndpoint InstantiateWithParameters(params object[] args)
        {
            return (IWebEndpoint)Activator.CreateInstance(HandlerType, args);
        }

        private IWebEndpoint TryInstantiateWithParameters(params object[] args)
        {
            try
            {
                return InstantiateWithParameters(args);
            }
#pragma warning disable CA1031
            catch (MissingMethodException)
            {
                return null;
            }
#pragma warning restore CA1031
        }

        public static RegisteredEndpoint Create<TEndpoint>(string method, string endpoint)
            where TEndpoint : IWebEndpoint
        {
            return new RegisteredEndpoint(method.ToUpper(), endpoint, typeof(TEndpoint));
        }

        public static RegisteredEndpoint Create<TEndpoint>(HttpMethod method, string endpoint)
            where TEndpoint : IWebEndpoint
        {
            var methodName = Enum
                .GetName(typeof(HttpMethod), method)?
                .ToUpper();

            return new RegisteredEndpoint(methodName, endpoint, typeof(TEndpoint));
        }

        public static RegisteredEndpoint CreateDefault<TEndpoint>()
            where TEndpoint : IWebEndpoint
        {
            return new RegisteredEndpoint("default", "default", typeof(TEndpoint));
        }

        public static RegisteredEndpoint CreateDefault(Type endpointType)
        {
            if (!typeof(IWebEndpoint).IsAssignableFrom(endpointType))
            {
                throw new ArgumentException(
                    $"must implement interface {nameof(IWebEndpoint)}");
            }

            return new RegisteredEndpoint("default", "default", endpointType);
        }
    }
}
