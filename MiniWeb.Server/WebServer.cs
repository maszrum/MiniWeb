using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server.Autofac;
using MiniWeb.Server.Endpoints;
using MiniWeb.Server.Requests;

namespace MiniWeb.Server
{
    public sealed class WebServer : IDisposable
    {
        private readonly string _basePath;
        private readonly HttpListener _listener;
        private readonly IContainer _diContainer;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public event Action<IWebRequest, Exception> ExceptionThrown;

        public List<IInterceptor> Interceptors { get; } = new List<IInterceptor>();

        public List<RegisteredEndpoint> Endpoints { get; }

        public RegisteredEndpoint DefaultEndpoint { get; set; }

        public IDependencyProvider DependencyProvider { get; }

        public WebServer(IContainer autofacContainer, string baseUrl, List<RegisteredEndpoint> endpoints)
        {
            _diContainer = autofacContainer ??
                throw new ArgumentNullException(nameof(autofacContainer));
            DependencyProvider = autofacContainer.ToDependencyProvider();

            Endpoints = endpoints ??
                throw new ArgumentNullException(nameof(endpoints));

            var uri = new Uri(baseUrl);
            _basePath = uri.AbsolutePath;

            _listener = new HttpListener();
            _listener.Prefixes.Add(baseUrl);
            _listener.IgnoreWriteExceptions = true;
        }

        public async Task OpenAsync()
        {
            _listener.Start();

            while (!_cts.IsCancellationRequested)
            {
                var request = await _listener.GetContextAsync();

                ProcessRequest(request);
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _listener.Close();
            _diContainer.Dispose();
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var response = context.Response;
            var request = context.Request;

            var endpointPath = GetEndpointPath(request.Url);
            var handler = GetHandler(endpointPath, request.HttpMethod);

            var requestObject = new BaseRequest(request, endpointPath);

            IWebResponse responseObject;
            try
            {
                responseObject = HandleBeginInterceptors(requestObject, handler.GetType());

                if (responseObject == null)
                {
                    responseObject = handler.Process(requestObject);
                    if (responseObject == null)
                    {
                        throw new ArgumentNullException(nameof(responseObject));
                    }
                }
            }
#pragma warning disable CA1031
            catch (Exception e)
            {
                var errorEndpoint = new InternalServerErrorEndpoint(e);
                responseObject = errorEndpoint.Process(requestObject);

                ExceptionThrown?.Invoke(requestObject, e);
            }
#pragma warning restore CA1031

            try
            {
                responseObject.ApplyTo(response);

                HandleEndInterceptors(requestObject, responseObject);
            }
#pragma warning disable CA1031
            catch (Exception e)
            {
                ExceptionThrown?.Invoke(requestObject, e);
            }
#pragma warning restore CA1031
        }

        private IWebResponse HandleBeginInterceptors(IWebRequest request, Type handlerType)
        {
            foreach (var interceptor in Interceptors)
            {
                var interceptorResponse = interceptor.BeforeProcessing(request, handlerType);
                if (interceptorResponse != null)
                {
                    return interceptorResponse;
                }
            }
            return null;
        }

        private void HandleEndInterceptors(IWebRequest request, IWebResponse response)
        {
            for (var i = Interceptors.Count - 1; i >= 0; i--)
            {
                var interceptorResponse = Interceptors[i].AfterProcessing(request, response);
                if (interceptorResponse != null)
                {
                    response = interceptorResponse;
                }
            }
        }

        private IWebEndpoint GetHandler(string endpointPath, string method)
        {
            IWebEndpoint GetDefaultOr404()
            {
                return DefaultEndpoint != null
                    ? DefaultEndpoint.Instantiate(DependencyProvider)
                    : new NotFoundEndpoint();
            }

            if (endpointPath == null)
            {
                return GetDefaultOr404();
            }

            var endpoints = Endpoints
                .Where(e => e.Endpoint == endpointPath)
                .ToArray();

            var endpoint = endpoints
                .SingleOrDefault(e => e.Method == method);
            
            if (endpoint == null)
            {
                return endpoints.Any()
                    ? new MethodNotAllowedEndpoint()
                    : GetDefaultOr404();
            }

            return endpoint.Instantiate(DependencyProvider);
        }

        private string GetEndpointPath(Uri url)
        {
            if (!url.AbsolutePath.StartsWith(_basePath))
            {
                return null;
            }

            return url.AbsolutePath
               .Substring(_basePath.Length)
               .Trim('/');
        }
    }
}
