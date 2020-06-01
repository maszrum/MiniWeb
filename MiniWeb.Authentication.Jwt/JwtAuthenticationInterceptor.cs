using System;
using System.Linq;
using System.Net;
using System.Reflection;
using MiniWeb.Core.Abstractions;
using MiniWeb.Server;
using MiniWeb.Server.Common;
using MiniWeb.Server.Responses;

namespace MiniWeb.Authentication.Jwt
{
    internal class JwtAuthenticationInterceptor<TData> : Interceptor
    {
        private readonly IAuthenticationService<TData> _authenticationService;
        private readonly JwtConverter _jwtConverter;

        public JwtAuthenticationInterceptor(
            IAuthenticationService<TData> authenticationService,
            JwtConverter jwtConverter)
        {
            _authenticationService = authenticationService ??
                throw new ArgumentNullException(nameof(authenticationService));

            _jwtConverter = jwtConverter ??
                throw new ArgumentNullException(nameof(jwtConverter));
        }

        public override IWebResponse BeforeProcessing(IWebRequest request, Type endpointType)
        {
            if (!AuthenticationNeed(endpointType))
            {
                return Authenticated();
            }

            var token = GetToken(request);
            if (token == null)
            {
                request.SetException(
                    new UnauthorizedAccessException("empty bearer token"));
                return Unauthenticated();
            }

            bool success;
            try
            {
                var claims = _jwtConverter.Decode(token);
                success = _authenticationService.VerifyAuthentication(claims, request);
            }
#pragma warning disable CA1031
            catch (Exception ex)
            {
                request.SetException(
                    new UnauthorizedAccessException($"invalid token: {token}, inner: {ex.GetType().Name}"));
                return Unauthenticated();
            }
#pragma warning restore CA1031

            if (!success)
            {
                request.SetException(
                    new UnauthorizedAccessException($"invalid login or password, bearer: {token}"));
                return Unauthenticated();
            }

            return Authenticated();
        }

        private static string GetToken(IWebRequest request)
        {
            var authorization = request.GetHeader(HttpRequestHeader.Authorization);

            if (authorization == null || !authorization.StartsWith("Bearer "))
            {
                return null;
            }

            var result = authorization.Substring(7);
            return result.Length > 0 ? result : null;
        }

        private static bool AuthenticationNeed(ICustomAttributeProvider handlerType)
        {
            var attribute = handlerType.GetCustomAttributes(typeof(AuthenticationRequired), true)
                .SingleOrDefault();

            if (attribute != null && attribute is AuthenticationRequired arAttribute)
            {
                return arAttribute.AuthenticationNeed;
            }

            return false;
        }

        private static IWebResponse Authenticated()
            => null;

        private static IWebResponse Unauthenticated()
            => new UnauthorizedResponse();
    }
}
