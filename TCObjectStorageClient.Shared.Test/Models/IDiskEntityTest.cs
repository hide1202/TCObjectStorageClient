using System;
using System.IO;
using TCObjectStorageClient.Models;
using Xunit;

namespace TCObjectStorageClient.Shared.Test.Models
{
    public class IDiskEntityTest
    {
        [Fact]
        public void BasePathTest()
        {
            // Arrange
            string dir = $"Models{Path.DirectorySeparatorChar}Resources";
            string path = $"{dir}{Path.DirectorySeparatorChar}Test.txt";

            // Act
            var entity = new DirectoryEntity(new DirectoryInfo(dir).FullName);
            var children = entity.GetAllChildren();

            // Assert
            Assert.True(File.Exists(path));
            foreach (var child in children)
            {
                Console.WriteLine($"Child : Parent[{child.parent}] PathFromBase[{child.pathFromBase}] Path[{child.entity.Path}]");
            }
            
            //Assert.AreEqual("Resources", entity.Path);
        }
    }
}
