using System.IO;
using Newtonsoft.Json;
using ToastCloud.ObjectStorage.Responses;
using Xunit;

namespace ToastCloudObjectStorageSdk.Test.Responses
{
    public class TokenResponseTest
    {
        private readonly string _json;

        public TokenResponseTest()
        {
            var path = Path.Combine("Responses", "response.json");
            _json = File.ReadAllText(path);
        }

        [Fact]
        public void DeserializeTest()
        {
            // Arrange
            // Act
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(_json);

            // Assert
            var token = tokenResponse.Access.Token;
            Assert.Equal("aaaaa-bbbbb-ccccc-dddd", token.Id);

            Assert.Equal(2014, token.IssuedAtUtc.Year);
            Assert.Equal(1, token.IssuedAtUtc.Month);
            Assert.Equal(30, token.IssuedAtUtc.Day);
            Assert.Equal(15, token.IssuedAtUtc.Hour);
            Assert.Equal(30, token.IssuedAtUtc.Minute);

            Assert.Equal(2014, token.ExpiresUtc.Year);
            Assert.Equal(1, token.ExpiresUtc.Month);
            Assert.Equal(31, token.ExpiresUtc.Day);
            Assert.Equal(15, token.ExpiresUtc.Hour);
            Assert.Equal(30, token.ExpiresUtc.Minute);
        }
    }
}
