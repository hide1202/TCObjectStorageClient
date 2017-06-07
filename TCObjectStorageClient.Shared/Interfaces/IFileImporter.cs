using System.Collections.Generic;
using System.IO;

namespace TCObjectStorageClient.Interfaces
{
    public interface IFileImporter
    {
        IList<string> GetFilePathList();

        DirectoryInfo GetDirectoryInfo();
    }
}
