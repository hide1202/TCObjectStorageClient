using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TCObjectStorageClient.ObjectStorage
{
    public interface IObjectStorage
    {
        Task<bool> UploadFile(string token, string account, string containerName, string objectName, byte[] body);
        
        Task<(bool, List<string>)> GetFiles(string token, string account, string containerName);

        Task<bool> DeleteFile(string token, string account, string containerName, string objectName);

        Task<bool> ContainsContainer(string token, string account, string containerName);
    }
}
