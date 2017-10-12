using System.Collections.Generic;
using System.Linq;
using ToastCloud.ObjectStorage.Responses;

namespace ToastCloud.ObjectStorage.SDKResponses
{
    public class FileInfoResult : IFileInfoResult
    {
        public List<FileInfo> Files { get; private set; }

        internal static FileInfoResult FromFileInfoResponseList(List<FileInfoResponse> responses)
        {
            return new FileInfoResult
            {
                Files = responses.Select(FileInfo.FromFileResponse).ToList()
            };
        }
    }
}
