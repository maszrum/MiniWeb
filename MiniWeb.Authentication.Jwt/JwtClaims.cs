using System.Collections.Generic;

namespace MiniWeb.Authentication.Jwt
{
    public class JwtClaims : Dictionary<string, object>
    {
        public JwtClaims()
        {
        }

        public JwtClaims(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }

        public const string ClientIdKey = "client_id";
        public string ClientId
        {
            get => Get<string>(ClientIdKey);
            set => Set(ClientIdKey, value);
        }

        public const string ExpiresKey = "exp";
        public double Expires
        {
            get => Get<double>(ExpiresKey);
            set => Set(ExpiresKey, value);
        }

        private T Get<T>(string key)
        {
            return (T)this[key];
        }

        private void Set<T>(string key, T value)
        {
            if (!ContainsKey(key))
            {
                Add(key, value);
            }
            else
            {
                this[key] = value;
            }
        }

        public IDictionary<string, object> AsDictionary() => this;
    }
}
