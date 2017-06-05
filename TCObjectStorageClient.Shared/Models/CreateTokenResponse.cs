using Newtonsoft.Json;

namespace TCObjectStorageClient.Models
{
    public class CreateTokenResponse
    {
        public class AccessResponse
        {
            [JsonProperty("token")]
            public Token Token { get; set; }
        }

        public class Token
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }

        [JsonProperty("access")]
        public AccessResponse Access;
    }
}
