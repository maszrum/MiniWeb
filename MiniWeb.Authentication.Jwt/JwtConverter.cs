using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace MiniWeb.Authentication.Jwt
{
    public class JwtConverter
    {
        private readonly JwtSettings _settings;
        private readonly JwtDecoder _decoder;
        private readonly JwtEncoder _encoder;

        public JwtConverter(JwtSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrWhiteSpace(_settings.Secret))
            {
                throw new ArgumentNullException(
                    $"{nameof(JwtSettings)}.Secret");
            }

            var serializer = new JsonNetSerializer();
            var dateProvider = new UtcDateTimeProvider();
            var validator = new JwtValidator(serializer, dateProvider);
            var urlEncoder = new JwtBase64UrlEncoder();
            var algorithm = new HMACSHA256Algorithm();

            _decoder = new JwtDecoder(serializer, validator, urlEncoder);
            _encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
        }

        public JwtClaims Decode(string token)
        {
            var dictionary = _decoder.DecodeToObject<IDictionary<string, object>>(
                token, _settings.Secret, true);

            return new JwtClaims(dictionary);
        }

        public string Encode(JwtClaims claims)
        {
            claims.Expires = DateTimeOffset.UtcNow
                .Add(_settings.Expires)
                .ToUnixTimeSeconds();

            return _encoder.Encode(claims.AsDictionary(), _settings.Secret);
        }
    }
}
