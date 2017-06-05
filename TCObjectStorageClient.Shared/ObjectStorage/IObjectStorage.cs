using System.Threading.Tasks;

namespace TCObjectStorageClient.ObjectStorage
{
    public interface IObjectStorage
    {
        Task<bool> UploadFile(string token, string account, string containerName, string objectName, byte[] body);
    }
}
