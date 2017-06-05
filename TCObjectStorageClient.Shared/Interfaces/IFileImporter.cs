using System.Collections.Generic;

namespace TCObjectStorageClient.Interfaces
{
    public interface IFileImporter
    {
        IList<string> GetFilePathList();
    }
}
