using System;
using Newtonsoft.Json;
using ToastCloud.ObjectStorage.Token;
using Xunit;

namespace ToastCloudObjectStorageSdk.Test.Token
{
    public class TokenTest
    {
        [Fact]
        public void DeserializeTest()
        {
            // Arrange
            var json = @"
{
   ""issued_at"":""2014-01-30T15:30:58.819584"",
   ""expires"":""2014-01-31T15:30:58Z"",
   ""id"":""aaaaa-bbbbb-ccccc-dddd"",
   ""tenant"":{
      ""description"":null,
      ""enabled"":true,
      ""id"":""fc394f2ab2df4114bde39905f800dc57"",
      ""name"":""demo""
   }
}";

            // Act
            var token = JsonConvert.DeserializeObject<TokenInfo>(json);

            // Assert
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

            Assert.Equal("aaaaa-bbbbb-ccccc-dddd", token.Id);
            Assert.Equal(null, token.Tenant.Description);
            Assert.Equal(true, token.Tenant.Enabled);
            Assert.Equal("fc394f2ab2df4114bde39905f800dc57", token.Tenant.Id);
            Assert.Equal("demo", token.Tenant.Name);
        }
    }
}
