using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniWeb.StandardHeaders
{
    internal class StandardHeadersContainer : IStandardHeaders
    {
        private readonly Dictionary<string, Func<string>> _factories = new Dictionary<string, Func<string>>();
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();
        
        public void RegisterFactory(string headerName, Func<string> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            ThrowIfAlreadyExists(headerName);

            _factories.Add(headerName, factory);
        }

        public void RegisterValue(string headerName, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            ThrowIfAlreadyExists(headerName);

            _values.Add(headerName, value);
        }

        public bool ContainsHeader(string headerName)
        {
            return _factories.ContainsKey(headerName)
                || _values.ContainsKey(headerName);
        }

        private void ThrowIfAlreadyExists(string headerName)
        {
            if (ContainsHeader(headerName))
            {
                throw new ArgumentException(
                    $"header is already registered: {headerName}", nameof(headerName));
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var value in _values)
            {
                yield return value;
            }
            foreach (var factory in _factories)
            {
                yield return new KeyValuePair<string, string>(
                    factory.Key, factory.Value());
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
