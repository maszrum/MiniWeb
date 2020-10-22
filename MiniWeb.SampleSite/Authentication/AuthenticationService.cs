using MiniWeb.Authentication.Jwt;
using MiniWeb.Core.Abstractions;
using MiniWeb.SampleSite.Models;

namespace MiniWeb.SampleSite.Authentication
{
    internal class AuthenticationService : IAuthenticationService<SignInModel>
    {
        private readonly JwtConverter _converter;

        public AuthenticationService(JwtConverter converter)
        {
            _converter = converter;
        }

        public bool VerifyAuthentication(JwtClaims claims, IRequestData requestData)
        {
            return claims.ClientId == "1";
        }

        public bool Authenticate(SignInModel input, out string token)
        {
            if (input.Username == "admin" && input.Password == "admin")
            {
                var claims = new JwtClaims()
                {
                    ClientId = "1"
                };

                token = _converter.Encode(claims);

                return true;
            }

            token = default;
            return false;
        }
    }
}
