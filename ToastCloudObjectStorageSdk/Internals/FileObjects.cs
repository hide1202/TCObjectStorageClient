using System.IO;
using System.Net;
using System.Threading.Tasks;
using Monad;
using ToastCloud.ObjectStorage.Constants;
using ToastCloud.ObjectStorage.Exceptions;
using ToastCloud.ObjectStorage.HttpRequest;
using ToastCloud.ObjectStorage.Token;

namespace ToastCloud.ObjectStorage.Internals
{
    internal class FileObjects : IFileObjects
    {
        public async Task<Try<bool>> UploadFile(TokenInfo token, string endPoint, string containerName, string objectName, Stream dataStream)
        {
            var client = new RestClient();
            client.AddHeader(HttpConstants.XAuthToken, token.Id);
            var @try = (await client.PutStreamAsync(ObjectStorageUrls.ObjectUrl(endPoint, containerName, objectName), dataStream))();
            if (@try.IsFaulted)
            {
                return () => new TryResult<bool>(@try.Exception);
            }
            var statusCode = @try.Value;
            if (statusCode == HttpStatusCode.Created)
                return () => new TryResult<bool>(true);
            return () => new TryResult<bool>(new GenericRequestException($"Fail to upload a [{objectName}] file"));
        }

        public async Task<Try<bool>> DeleteFile(TokenInfo token, string endPoint, string containerName, string objectName)
        {
            var client = new RestClient();
            client.AddHeader(HttpConstants.XAuthToken, token.Id);
            var @try = (await client.DeleteAsync(ObjectStorageUrls.ObjectUrl(endPoint, containerName, objectName)))();
            if (@try.IsFaulted)
            {
                return () => new TryResult<bool>(@try.Exception);
            }
            var statusCode = @try.Value;
            if (statusCode == HttpStatusCode.NoContent)
                return () => new TryResult<bool>(true);
            return () => new TryResult<bool>(new GenericRequestException($"Fail to delete a {objectName} file"));
        }
    }
}
