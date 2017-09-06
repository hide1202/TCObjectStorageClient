using Newtonsoft.Json;
using ToastCloud.ObjectStorage.Token;
using Xunit;

namespace ToastCloudObjectStorageSdk.Test.Token
{
    public class ServiceCatalogTest
    {
        [Fact]
        public void DeserializeTest()
        {
            // Arrange
            var json = @"
{
    ""endpoints"": [
        {
            ""region"": ""RegionOne"",
            ""publicURL"": ""http://23.253.72.207:8774/v2/fc394f2ab2df4114bde39905f800dc57""
        }
    ],
    ""type"": ""compute"",
    ""name"": ""nova""
}";

            // Act
            var serviceCatalog = JsonConvert.DeserializeObject<ServiceCatalogInfo>(json);

            // Assert
            Assert.Equal(1, serviceCatalog.EndPoints.Length);
            Assert.Equal("RegionOne", serviceCatalog.EndPoints[0].Region);
            Assert.Equal("http://23.253.72.207:8774/v2/fc394f2ab2df4114bde39905f800dc57", serviceCatalog.EndPoints[0].PublicUrl);
            Assert.Equal("compute", serviceCatalog.Type);
            Assert.Equal("nova", serviceCatalog.Name);
        }
    }
}
