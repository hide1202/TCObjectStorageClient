using Newtonsoft.Json;

namespace ToastCloud.ObjectStorage.Token
{
    public class AccessInfo
    {
        [JsonProperty("token")]
        public TokenInfo Token { get; set; }

        [JsonProperty("serviceCatalog")]
        public ServiceCatalogInfo[] ServiceCatalog { get; set; }

        public override string ToString()
        {
            return $"{nameof(Token)}: {Token}";
        }
    }
}
