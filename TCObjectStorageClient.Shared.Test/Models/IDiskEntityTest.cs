using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TCObjectStorageClient.Models;

namespace TCObjectStorageClient.Shared.Test.Models
{
    [TestClass]
    public class IDiskEntityTest
    {
        [TestMethod]
        public void Test1()
        {
            // Arrange
            string dir = $"Models{Path.DirectorySeparatorChar}Resources";
            string path = $"{dir}{Path.DirectorySeparatorChar}Test.txt";

            // Act
            var entity = new DirectoryEntity(new DirectoryInfo(dir).FullName);
            var children = entity.GetAllChildren();

            // Assert
            Assert.IsTrue(File.Exists(path));
            foreach (var child in children)
            {
                Console.WriteLine($"Child : Parent[{child.parent}] PathFromBase[{child.pathFromBase}] Path[{child.entity.Path}]");
            }
            
            //Assert.AreEqual("Resources", entity.Path);
        }
    }
}
