using APIHandler.Extensions;
using APIHandler.Models;

namespace APIHandler
{
    public static class APIHandlerService<T> where T : class
    {

        /// <summary>
        /// To handle all post calls
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<APIResult<T>> PostHandlerService(string payload, string? url, Dictionary<string, string>? parameters = null)
        {
            url = Constants.BuidServiceURL(url, parameters);

            return await POSTHandler<T>.PostAPI(payload, url);
        }
    }
}
