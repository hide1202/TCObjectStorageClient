using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monad;
using ToastCloud.ObjectStorage.Constants;
using ToastCloud.ObjectStorage.Exceptions;
using ToastCloud.ObjectStorage.HttpRequest;
using ToastCloud.ObjectStorage.Requests;
using ToastCloud.ObjectStorage.Responses;

namespace ToastCloud.ObjectStorage.Internals
{
    internal class IdentityAuthenticate : IAuthenticate
    {
        private class CacheEqualityComparer : IEqualityComparer<TokenRequest>
        {
            public bool Equals(TokenRequest x, TokenRequest y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null || y == null)
                    return false;
                return x.IsSame(y);
            }

            public int GetHashCode(TokenRequest req)
            {
                return req.Auth.TenantName.GetHashCode()
                    ^ req.Auth.PasswordCredentials.UserName.GetHashCode()
                    ^ req.Auth.PasswordCredentials.Password.GetHashCode();
            }
        }

        private readonly Dictionary<TokenRequest, TokenResponse> _cache;
        private readonly IRestClient _client;

        public IdentityAuthenticate()
        {
            _cache = new Dictionary<TokenRequest, TokenResponse>(new CacheEqualityComparer());
        }

        internal IdentityAuthenticate(IRestClient client) : this()
        {
            _client = client;
        }

        public async Task<Try<TokenResponse>> Authenticate(TokenRequest request)
        {
            if (_cache.ContainsKey(request))
            {
                var cachedRes = _cache[request];
                var expireLocalDate = cachedRes.Access.Token.Expires;
                var nowLocalTime = DateTime.Now;

                if (nowLocalTime < expireLocalDate)
                {
                    return () => cachedRes;
                }
            }

            var restClient = _client ?? new RestClient();
            var tryTokenResponse = await restClient.PostAsync<TokenRequest, TokenResponse>(UrlConstants.IdentityUrl, request);
            var tryResultTokenResponse = tryTokenResponse();
            if (tryResultTokenResponse.IsFaulted)
                return () => new TryResult<TokenResponse>(tryResultTokenResponse.Exception);

            var restResponse = tryResultTokenResponse.Value;
            var optionTokenResponse = restResponse.Content();
            var token = optionTokenResponse.GetValueOrDefault()?.Access?.Token;
            if (token != null)
            {
                _cache[request] = optionTokenResponse.Value;
                return () => optionTokenResponse.Value;
            }

            return () => new TryResult<TokenResponse>(new InvalidTokenException());
        }
    }
}
