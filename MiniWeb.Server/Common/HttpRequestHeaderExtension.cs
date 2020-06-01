using System.Net;
using System.Text;

namespace MiniWeb.Server.Common
{
    public static class HttpRequestHeaderExtension
    {
        private const char Separator = '-';

        public static string ToHeaderName(this HttpRequestHeader enumValue)
        {
            var enumName = enumValue.ToString();
            return Convert(enumName);
        }

        public static string ToHeaderName(this HttpResponseHeader enumValue)
        {
            var enumName = enumValue.ToString();
            return Convert(enumName);
        }

        private static string Convert(string enumName)
        {
            var sb = new StringBuilder();

            var ch = enumName[0];
            sb.Append(ch);

            for (var i = 1; i < enumName.Length; i++)
            {
                ch = enumName[i];
                if (char.IsUpper(ch))
                {
                    sb.Append(Separator);
                }
                sb.Append(ch);
            }

            var result = sb.ToString();

            return result.Length == 2 ? result.ToUpper() : result;
        }
    }
}
