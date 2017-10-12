using System.IO;
using System.Threading.Tasks;
using Monad;
using ToastCloud.ObjectStorage.Token;

namespace ToastCloud.ObjectStorage.Internals
{
    interface IFileObjects
    {
        Task<Try<bool>> UploadFile(TokenInfo token, string endPoint, string containerName, string objectName, Stream dataStream);

        Task<Try<bool>> DeleteFile(TokenInfo token, string endPoint, string containerName, string objectName);
    }
}
