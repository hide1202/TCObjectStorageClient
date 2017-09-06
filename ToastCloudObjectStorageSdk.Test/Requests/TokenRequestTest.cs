using Newtonsoft.Json;
using ToastCloud.ObjectStorage.Requests;
using Xunit;

namespace ToastCloudObjectStorageSdk.Test.Requests
{
    public class TokenRequestTest
    {
        [Fact]
        public void DeserializeTest()
        {
            // Arrange
            var expected = @"{""auth"":{""tenantName"":""abcdefg"",""passwordCredentials"":{""username"":""test@nhnent.com"",""password"":""test""}}}";
            var request = new TokenRequest("abcdefg", "test@nhnent.com", "test");

            // Act
            var requestJson = JsonConvert.SerializeObject(request, Formatting.None);

            // Assert
            Assert.Equal(expected, requestJson);
        }
    }
}
