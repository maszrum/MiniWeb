using System;
using System.Collections.Generic;
using Autofac;
using MiniWeb.Core;
using MiniWeb.Core.Abstractions;

namespace MiniWeb.Server
{
	public class WebServerBuilder
	{
		private readonly List<RegisteredEndpoint> _registeredEndpoints = new List<RegisteredEndpoint>();
		private readonly List<Type> _registeredInterceptors = new List<Type>();
		private readonly List<Action<WebServer>> _buildCallbacks = new List<Action<WebServer>>();
		
		private string _baseUrl;

		public ContainerBuilder ContainerBuilder { get; }

		public WebServerBuilder(ContainerBuilder autofacContainerBuilder)
		{
			ContainerBuilder = autofacContainerBuilder ??
				throw new ArgumentNullException(nameof(autofacContainerBuilder));
		}

		public WebServerBuilder WithBaseUrl(string url)
		{
			if (_baseUrl != null)
			{
				throw new InvalidOperationException(
					"method can be called once");
			}

			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentNullException(nameof(url));
			}
			if (!url.EndsWith("/"))
			{
				url += '/';
			}

			_baseUrl = url;

			return this;
		}

		public WebServerBuilder AddEndpoint<TEndpoint>(HttpMethod method, string endpoint)
			where TEndpoint : IWebEndpoint
		{
			var newEndpoint = RegisteredEndpoint.Create<TEndpoint>(method, endpoint.Trim('/'));
			_registeredEndpoints.Add(newEndpoint);

			return this;
		}
		
		public WebServerBuilder AddInterceptor<TInterceptor>()
			where TInterceptor : IInterceptor
		{
			var interceptorType = typeof(TInterceptor);

			ThrowIfInterceptorExists(interceptorType);

			ContainerBuilder.RegisterType<TInterceptor>()
				.As<IInterceptor>();

			_registeredInterceptors.Add(interceptorType);

			return this;
		}

		public WebServerBuilder AddInterceptor(IInterceptor interceptor)
		{
			if (interceptor == null)
			{
				throw new ArgumentNullException(nameof(interceptor));
			}

			var interceptorType = interceptor.GetType();

			ThrowIfInterceptorExists(interceptorType);
			
			ContainerBuilder.RegisterInstance(interceptor)
				.As<IInterceptor>();

			_registeredInterceptors.Add(interceptorType);

			return this;
		}

		public WebServerBuilder RegisterBuildCallback(Action<WebServer> callback)
		{
			_buildCallbacks.Add(callback);

			return this;
		}

		public WebServer Build()
		{
			if (_baseUrl == null)
			{
				throw new ArgumentNullException(
					nameof(_baseUrl), $"use {nameof(WithBaseUrl)} method");
			}

			var container = ContainerBuilder.Build();
			
			var server = new WebServer(container, _baseUrl, _registeredEndpoints);

			InvokeBuildCallbacks(server);

			ResolveAndAddInterceptors(container, server);

			return server;
		}

		private static void ResolveAndAddInterceptors(IComponentContext container, WebServer server)
		{
			if (container.TryResolve(out IEnumerable<IInterceptor> interceptors))
			{
				server.Interceptors.AddRange(interceptors);
			}
		}

		private void InvokeBuildCallbacks(WebServer server)
		{
			foreach (var callback in _buildCallbacks)
			{
				callback(server);
			}
		}

		private void ThrowIfInterceptorExists(Type interceptorType)
		{
			if (_registeredInterceptors.Contains(interceptorType))
			{
				throw new ArgumentException(
					$"interceptor of specified type is already registered: {interceptorType.Name}");
			}
		}
	}
}
