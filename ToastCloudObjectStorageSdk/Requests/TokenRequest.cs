using Newtonsoft.Json;

namespace ToastCloud.ObjectStorage.Requests
{
    internal class TokenRequest
    {
        internal class PasswordCredentials
        {
            public PasswordCredentials(string userName, string password)
            {
                UserName = userName;
                Password = password;
            }

            [JsonProperty("username")]
            internal string UserName { get; }

            [JsonProperty("password")]
            internal string Password { get; }
        }

        internal class AuthInfo
        {
            public AuthInfo(string tenantName, string userName, string password)
            {
                TenantName = tenantName;
                PasswordCredentials = new PasswordCredentials(userName, password);
            }

            [JsonProperty("tenantName")]
            internal string TenantName { get; }

            [JsonProperty("passwordCredentials")]
            internal PasswordCredentials PasswordCredentials { get; }
        }
        
        internal TokenRequest(string tenantName, string userName, string password)
        {
            Auth = new AuthInfo(tenantName, userName, password);
        }

        [JsonProperty("auth")]
        internal AuthInfo Auth { get; }

        internal bool IsSame(TokenRequest request)
        {
            return this.Auth.TenantName == request.Auth.TenantName &&
                   this.Auth.PasswordCredentials.UserName == request.Auth.PasswordCredentials.UserName &&
                   this.Auth.PasswordCredentials.Password == request.Auth.PasswordCredentials.Password;
        }
    }
}
