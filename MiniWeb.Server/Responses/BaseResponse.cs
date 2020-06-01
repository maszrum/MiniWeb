using System;
using System.Collections.Generic;
using System.Net;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server.Common;

namespace MiniWeb.Server.Responses
{
    public abstract class BaseResponse : IWebResponse
    {
        private readonly List<Action<HttpListenerResponse>> _headersActions = new List<Action<HttpListenerResponse>>();

        public abstract HttpStatusCode StatusCode { get; set; }

        protected abstract void Rewrite(HttpListenerResponse response);

        public void ApplyTo(HttpListenerResponse response)
        {
            foreach (var headerAction in _headersActions)
            {
                headerAction(response);
            }

            Rewrite(response);

            response.StatusCode = (int)StatusCode;

            response.Close();
        }

        public void SetHeader(string header, string value)
        {
            _headersActions.Add(response =>
            {
                response.Headers.Set(header, value);
            });
        }

        public void SetHeader(HttpResponseHeader header, string value)
        {
            SetHeader(header.ToHeaderName(), value);
        }
    }
}
