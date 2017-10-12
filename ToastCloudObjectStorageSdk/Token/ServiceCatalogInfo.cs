using Newtonsoft.Json;

namespace ToastCloud.ObjectStorage.Token
{
    public class ServiceCatalogInfo
    {
        [JsonProperty("endpoints")]
        public EndPoint[] EndPoints { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
