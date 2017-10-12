using System;
using System.Net;
using System.Threading.Tasks;
using Monad;
using NSubstitute;
using ToastCloud.ObjectStorage.HttpRequest;
using ToastCloud.ObjectStorage.Internals;
using ToastCloud.ObjectStorage.Requests;
using ToastCloud.ObjectStorage.Responses;
using ToastCloud.ObjectStorage.Token;
using Xunit;
using EndPoint = ToastCloud.ObjectStorage.Token.EndPoint;

namespace ToastCloudObjectStorageSdk.Test.Internals
{
    public class IdentityAuthenticateTest
    {
        [Fact]
        public void AuthenticateCacheTest()
        {
            // Arrange
            var request = new TokenRequest("", "", "");
            var restClient = Substitute.For<IRestClient>();
            var mockResult = new TokenResponse
            {
                Access = new AccessInfo
                {
                    Token = new TokenInfo
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(1),
                        Id = DateTime.Now.ToString(),
                        IssuedAtUtc = DateTime.UtcNow,
                    }
                }
            };

            TryResult<RestResponse<TokenResponse>> ToTryResponse()
            {
                var restResponse = new RestResponse<TokenResponse>(HttpStatusCode.OK, mockResult);
                var tryResult = new TryResult<RestResponse<TokenResponse>>(restResponse);
                return tryResult;
            }

            restClient.PostAsync<TokenRequest, TokenResponse>(Arg.Any<string>(), Arg.Any<TokenRequest>())
                .ReturnsForAnyArgs(Task.FromResult((Try<RestResponse<TokenResponse>>)ToTryResponse));
            var auth = new IdentityAuthenticate(restClient);

            // Act
            var res1 = auth.Authenticate(request).Result;
            var res2 = auth.Authenticate(request).Result;

            // Assert
            Assert.Equal(res1().Value.Access.Token.Id, res2().Value.Access.Token.Id);
            restClient.Received(1).PostAsync<TokenRequest, TokenResponse>(Arg.Any<string>(), Arg.Any<TokenRequest>());
        }
    }
}
