using Newtonsoft.Json;

namespace ToastCloud.ObjectStorage.Responses
{
    internal class ContainerInfoResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
