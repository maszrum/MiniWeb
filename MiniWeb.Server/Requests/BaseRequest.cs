using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using MiniWeb.Core;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server.Common;

namespace MiniWeb.Server.Requests
{
    public class BaseRequest : IWebRequest
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public Guid Id { get; } = Guid.NewGuid();

        protected HttpListenerRequest Request { get; }

        public long ContentLength => Request.ContentLength64;

        public Stream InputStream => Request.InputStream;

        private readonly string[] _route;
        public IReadOnlyList<string> Route => _route;

        public NameValueCollection QueryString => Request.QueryString;

        public HttpMethod Method { get; }

        public IPAddress RemoteAddress => Request.RemoteEndPoint?.Address;

        public string RawUrl => Request.RawUrl;

        public BaseRequest(HttpListenerRequest request, string path)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));

            _route = path.Split(new[] { '/' });

            Method = HttpMethodConverter.Convert(request.HttpMethod);
        }

        public BaseRequest(BaseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            Request = request.Request;
        }

        public string GetHeader(string header)
        {
            return Request.Headers.Get(header);
        }

        public string GetHeader(HttpRequestHeader header)
        {
            return GetHeader(header.ToHeaderName());
        }

        public void SetData<T>(string key, T data)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = data;
            }
            else
            {
                _data.Add(key, data);
            }
        }

        public T GetData<T>(string key)
        {
            if (_data.TryGetValue(key, out var data))
            {
                if (data is T typedData)
                {
                    return typedData;
                }

                throw new InvalidCastException(
                    $"cannot cast data[{key}] to type {typeof(T).Name}");
            }

            throw new KeyNotFoundException(
                "data with specified key was not found");
        }

        public bool TryGetData<T>(string key, out T data)
        {
            if (_data.TryGetValue(key, out var dataObject) &&
                dataObject is T typedData)
            {
                data = typedData;
                return true;
            }

            data = default;
            return false;
        }
    }
}
