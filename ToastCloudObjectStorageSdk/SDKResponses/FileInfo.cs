using System;
using ToastCloud.ObjectStorage.Responses;

namespace ToastCloud.ObjectStorage.SDKResponses
{
    public class FileInfo
    {
        public string Hash { get; private set; }

        public DateTime LastModified { get; private set; }

        public long Bytes { get; private set; }

        public string Name { get; private set; }

        public string ContentType { get; private set; }

        internal static FileInfo FromFileResponse(FileInfoResponse response)
        {
            return new FileInfo
            {
                Hash = response.Hash,
                LastModified = response.LastModified,
                Bytes = response.Bytes,
                Name = response.Name,
                ContentType = response.ContentType
            };
        }
    }
}
