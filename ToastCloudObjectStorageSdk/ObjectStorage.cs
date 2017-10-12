using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Monad;
using ToastCloud.ObjectStorage.Exceptions;
using ToastCloud.ObjectStorage.Internals;
using ToastCloud.ObjectStorage.Requests;
using ToastCloud.ObjectStorage.Responses;
using ToastCloud.ObjectStorage.SDKResponses;

namespace ToastCloud.ObjectStorage
{
    public class ObjectStorage
    {
        private const string FailGetEndPointMessage = "Fail to get endpoint";

        private TokenResponse _tokenResponse;

        public bool IsAuthenticate => _tokenResponse?.Access?.Token != null;

        private Option<string> GetEndPoint()
        {
            if (!IsAuthenticate)
                return Option.Nothing<string>();
            var serviceCatalog = _tokenResponse?.Access?.ServiceCatalog;
            if (serviceCatalog == null || serviceCatalog.Length < 0)
                return Option.Nothing<string>();
            if ((serviceCatalog[0]?.EndPoints?.Length ?? 0) < 0)
                return Option.Nothing<string>();
            return () => serviceCatalog[0].EndPoints[0].PublicUrl;
        }

        public async Task<Result<bool, Exception>> Authenticate(string tenantName, string userName, string password)
        {
            var authenticate = new IdentityAuthenticate();
            var tryResult = (await authenticate.Authenticate(new TokenRequest(tenantName, userName, password)))();
            _tokenResponse = tryResult.IsFaulted ? null : tryResult.Value;
            return Result<bool, Exception>.FromTry(tryResult, res => res?.Access?.Token != null);
        }

        public async Task<Result<IContainerInfoResult, Exception>> ContainerList()
        {
            if (!IsAuthenticate)
                return Result<IContainerInfoResult, Exception>.ToFailure(new UnauthorizedRequestException());
            if (!GetEndPoint().HasValue())
                return Result<IContainerInfoResult, Exception>.ToFailure(new ObjectStorageSdkException(FailGetEndPointMessage));

            var endPoint = GetEndPoint().Value();
            var containers = new Containers();
            var @try = (await containers.ContainerList(endPoint, _tokenResponse.Access.Token))();
            return Result<IContainerInfoResult, Exception>.FromTry(@try, ContainerInfoResult.FromResponse);
        }

        public async Task<Result<IFileInfoResult, Exception>> FileList(string containerName)
        {
            if (!IsAuthenticate)
                return Result<IFileInfoResult, Exception>.ToFailure(new UnauthorizedRequestException());
            if (!GetEndPoint().HasValue())
                return Result<IFileInfoResult, Exception>.ToFailure(new ObjectStorageSdkException(FailGetEndPointMessage));

            var endPoint = GetEndPoint().Value();
            var containers = new Containers();
            var @try = (await containers.FileList(endPoint, _tokenResponse.Access.Token, containerName))();
            return Result<IFileInfoResult, Exception>.FromTry(@try, FileInfoResult.FromFileInfoResponseList);
        }

        public async Task<Result<bool, Exception>> UploadFile(string containerName, string objectName, Stream dataStream)
        {
            if (!IsAuthenticate)
                return Result<bool, Exception>.ToFailure(new UnauthorizedRequestException());
            if (!GetEndPoint().HasValue())
                return Result<bool, Exception>.ToFailure(new ObjectStorageSdkException(FailGetEndPointMessage));

            var endPoint = GetEndPoint().Value();
            var fileObjects = new FileObjects();
            var @try = (await fileObjects.UploadFile(_tokenResponse.Access.Token, endPoint, containerName, objectName, dataStream))();
            return Result<bool, Exception>.FromTry(@try);
        }

        public async Task<Result<bool, Exception>> DeleteFile(string containerName, string objectName)
        {
            if (!IsAuthenticate)
                return Result<bool, Exception>.ToFailure(new UnauthorizedRequestException());
            if (!GetEndPoint().HasValue())
                return Result<bool, Exception>.ToFailure(new ObjectStorageSdkException(FailGetEndPointMessage));

            var endPoint = GetEndPoint().Value();
            var fileObjects = new FileObjects();
            var @try = (await fileObjects.DeleteFile(_tokenResponse.Access.Token, endPoint, containerName, objectName))();
            return Result<bool, Exception>.FromTry(@try);
        }

        public async Task<Result<bool, Exception>> ContainsContainer(string containerName)
        {
            var result = await ContainerList();
            if(!result.IsSuccess)
                return Result<bool, Exception>.ToFailure(result.Cause);

            var isContains = result.Value?.Containers?.Any(c => c.Name == containerName) ?? false;
            return Result<bool, Exception>.ToSuccess(isContains);
        }
    }
}
