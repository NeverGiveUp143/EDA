using APIHandler.Models;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace APIHandler.Extensions
{
    public static class POSTHandler<T> where T : class
    {

        /// <summary>
        /// Handle Post calls
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<APIResult<T>> PostAPI(string payload, string? url)
        {

            APIResult<T> result = new APIResult<T>();

            if (string.IsNullOrEmpty(url))
                return result;

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,

                ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            };

            using (var client = new HttpClient(handler))
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(url),
                        Content = new StringContent(payload, UnicodeEncoding.UTF8, Constants.ApplicationJson)
                    };
                    var postTask = await client.SendAsync(request);
                    string apiResponse = await postTask.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<APIResult<T>>(apiResponse) ?? new APIResult<T>();
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.HttpStatus = EnumHelper.ToEnum<HttpStatusCode>(HttpStatusCode.InternalServerError.ToString());
                }
            }

            return result;
        }

    }
}
