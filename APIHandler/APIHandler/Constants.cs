using System.Net;
using System.Text;

namespace APIHandler
{
    public class Constants
    {
        public static string Ampersand = "&";
        public static string ApplicationJson = "application/json";
        public static string? BuidServiceURL(string? baseUrl, Dictionary<string, string>? parameters)
        {
            if (parameters != null && parameters.Count > 0 && !string.IsNullOrWhiteSpace(baseUrl))
            {
                StringBuilder url = new StringBuilder(baseUrl);
                url.Append(Ampersand);

                foreach (var para in parameters)
                {
                    url.Append($"{WebUtility.UrlEncode(para.Key)}={WebUtility.UrlEncode(para.Value)}&");
                }

                url.Remove(url.Length - 1, 1);

                return url.ToString();
            }

            return baseUrl;
        }
    }
}
