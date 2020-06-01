using System;

namespace MiniWeb.Authentication.Jwt
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AuthenticationRequired : Attribute
    {
        public bool AuthenticationNeed { get; set; }

        public AuthenticationRequired() : this(true)
        {
        }

        public AuthenticationRequired(bool authenticationNeed)
        {
            AuthenticationNeed = authenticationNeed;
        }
    }
}
