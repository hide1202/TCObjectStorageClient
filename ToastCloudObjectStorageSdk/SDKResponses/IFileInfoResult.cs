using System.Collections.Generic;

namespace ToastCloud.ObjectStorage.SDKResponses
{
    public interface IFileInfoResult
    {
        List<FileInfo> Files { get; }
    }
}
