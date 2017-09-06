using System.Collections.Generic;
using System.Threading.Tasks;
using Monad;
using ToastCloud.ObjectStorage.Constants;
using ToastCloud.ObjectStorage.Exceptions;
using ToastCloud.ObjectStorage.HttpRequest;
using ToastCloud.ObjectStorage.Responses;
using ToastCloud.ObjectStorage.Token;

namespace ToastCloud.ObjectStorage.Internals
{
    internal class Containers : IContainers
    {
        public async Task<Try<List<ContainerInfoResponse>>> ContainerList(string endPoint, TokenInfo token)
        {
            var client = new RestClient();
            client.AddHeader(HttpConstants.XAuthToken, token.Id);
            var @try = (await client.GetAsync<List<ContainerInfoResponse>>(endPoint))();
            if (@try.IsFaulted)
            {
                return () => new TryResult<List<ContainerInfoResponse>>(@try.Exception);
            }
            var restResponse = @try.Value;
            var result = restResponse.Content();
            if (result.HasValue)
                return () => new TryResult<List<ContainerInfoResponse>>(result.Value);
            return () => new TryResult<List<ContainerInfoResponse>>(new GenericRequestException("Fail to receive container list"));
        }

        public async Task<Try<List<FileInfoResponse>>> FileList(string endPoint, TokenInfo token, string containerName)
        {
            var client = new RestClient();
            client.AddHeader(HttpConstants.XAuthToken, token.Id);
            var @try = (await client.GetAsync<List<FileInfoResponse>>(ObjectStorageUrls.ContainerUrl(endPoint, containerName)))();
            if (@try.IsFaulted)
            {
                return () => new TryResult<List<FileInfoResponse>>(@try.Exception);
            }
            var restResponse = @try.Value;
            var result = restResponse.Content();
            if (result.HasValue)
                return () => new TryResult<List<FileInfoResponse>>(result.Value);
            return () => new TryResult<List<FileInfoResponse>>(new GenericRequestException("Fail to receive file list"));
        }
    }
}
