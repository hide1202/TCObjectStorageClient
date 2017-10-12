using System;
using Newtonsoft.Json;

namespace ToastCloud.ObjectStorage.Responses
{
    internal class FileInfoResponse
    {
        [JsonProperty("hash")]
        internal string Hash { get; set; }

        [JsonProperty("last_modified")]
        internal DateTime LastModified { get; set; }

        [JsonProperty("bytes")]
        internal long Bytes { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        [JsonProperty("content_type")]
        internal string ContentType { get; set; }
    }
}
