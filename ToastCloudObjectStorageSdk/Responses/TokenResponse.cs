using Newtonsoft.Json;
using ToastCloud.ObjectStorage.Token;

namespace ToastCloud.ObjectStorage.Responses
{
    public class TokenResponse
    {
        [JsonProperty("access")]
        public AccessInfo Access { get; set; }

        public override string ToString()
        {
            return $"{nameof(Access)}: {Access}";
        }
    }
}
