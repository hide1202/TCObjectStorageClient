using Newtonsoft.Json;

namespace ToastCloud.ObjectStorage.Token
{
    public class EndPoint
    {
        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("publicURL")]
        public string PublicUrl { get; set; }
    }
}
