using System;
using System.IO;
using TCObjectStorageClient.Models;
using Xunit;
using Xunit.Abstractions;

namespace TCObjectStorageClient.Shared.Test.Models
{
    public class IDiskEntityTest
    {
        private readonly ITestOutputHelper _output;

        public IDiskEntityTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void BasePathTest()
        {
            // Arrange
            string dir = $"Models{Path.DirectorySeparatorChar}Resources";

            // Act
            var entity = new DirectoryEntity(new DirectoryInfo(dir).FullName);
            var children = entity.GetAllChildren();

            // Assert
            foreach (var child in children)
            {
                _output.WriteLine($"Child : Parent[{child.parent}] PathFromBase[{child.pathFromBase}] Path[{child.entity.Path}]");
            }
        }
    }
}
