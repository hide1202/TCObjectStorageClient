using System;
using Newtonsoft.Json;

namespace ToastCloud.ObjectStorage.Token
{
    public class Tenant
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class TokenInfo
    {
        [JsonProperty("issued_at")]
        public DateTime IssuedAtUtc { get; set; }

        [JsonProperty("expires")]
        public DateTime ExpiresUtc { get; set; }

        public DateTime IssuedAt => TimeZone.CurrentTimeZone.ToLocalTime(IssuedAtUtc);

        public DateTime Expires => TimeZone.CurrentTimeZone.ToLocalTime(ExpiresUtc);

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tenant")]
        public Tenant Tenant { get; set; }

        public override string ToString()
        {
            return $"{nameof(IssuedAt)}: {IssuedAt}, {nameof(Expires)}: {Expires}, {nameof(Id)}: {Id}";
        }
    }
}
