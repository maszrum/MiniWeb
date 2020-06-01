using System;

namespace MiniWeb.Authentication.Jwt
{
    public class JwtSettings
    {
        private TimeSpan _expires = TimeSpan.FromHours(1);
        public TimeSpan Expires
        {
            get => _expires;
            set
            {
                if (value.TotalHours <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(Expires), "must be positive");
                }
                _expires = value;
            }
        }

        public string Secret { get; set; }
    }
}
