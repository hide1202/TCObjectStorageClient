using System.Collections.Generic;
using System.Threading.Tasks;
using Monad;
using ToastCloud.ObjectStorage.Responses;
using ToastCloud.ObjectStorage.Token;

namespace ToastCloud.ObjectStorage.Internals
{
    internal interface IContainers
    {
        Task<Try<List<ContainerInfoResponse>>> ContainerList(string endPoint, TokenInfo token);

        Task<Try<List<FileInfoResponse>>> FileList(string endPoint, TokenInfo token, string containerName);
    }
}
