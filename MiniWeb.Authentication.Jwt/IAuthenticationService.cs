using MiniWeb.Core.Abstractions;

namespace MiniWeb.Authentication.Jwt
{
    public interface IAuthenticationService<in TData>
    {
        bool VerifyAuthentication(JwtClaims claims, IRequestData requestData);
        bool Authenticate(TData input, out string token);
    }
}
