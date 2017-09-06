using System.Threading.Tasks;
using Monad;
using ToastCloud.ObjectStorage.Requests;
using ToastCloud.ObjectStorage.Responses;

namespace ToastCloud.ObjectStorage.Internals
{
    internal interface IAuthenticate
    {
        Task<Try<TokenResponse>> Authenticate(TokenRequest request);
    }
}
