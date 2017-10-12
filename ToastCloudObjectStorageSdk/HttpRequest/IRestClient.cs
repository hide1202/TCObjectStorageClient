using System.IO;
using System.Net;
using System.Threading.Tasks;
using Monad;

namespace ToastCloud.ObjectStorage.HttpRequest
{
    internal interface IRestClient
    {
        void AddHeader(string key, string value);

        Task<Try<RestResponse<TResponseBody>>> PostAsync<TRequestBody, TResponseBody>(string url, TRequestBody request);

        Task<Try<HttpStatusCode>> PutStreamAsync(string url, Stream stream);

        Task<Try<HttpStatusCode>> DeleteAsync(string url);

        Task<Try<RestResponse<TResponseBody>>> GetAsync<TResponseBody>(string url, params (string key, string value)[] querys);
    }
}
