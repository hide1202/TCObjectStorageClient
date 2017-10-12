using System.Net;
using Monad;

namespace ToastCloud.ObjectStorage.HttpRequest
{
    internal class RestResponse<T>
    {
        internal RestResponse(HttpStatusCode statusCode, T responseContent)
        {
            StatusCode = statusCode;
            if (responseContent == null)
            {
                Content = Option.Nothing<T>();
            }
            else
            {
                Content = () => responseContent;
            }
        }

        internal HttpStatusCode StatusCode { get; }

        internal Option<T> Content { get; }
    }
}
