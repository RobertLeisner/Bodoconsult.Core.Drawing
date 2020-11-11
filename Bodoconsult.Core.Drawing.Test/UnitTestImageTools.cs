using System.IO;
using Bodoconsult.Core.Drawing.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Core.Drawing.Test
{
    public class UnitTestImageTools
    {


        //[SetUp]
        //public void Setup()
        //{

        //}


        //[TearDown]
        //public void Cleanup()
        //{
        //}


        [Test]
        public void TestGetImageSize()
        {

            // Arrange
            var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");

            // Act
            var result = ImageTools.GetImageSize(source);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Width > 0);
            Assert.IsTrue(result.Height > 0);

        }

        [Test]
        public void TestGenerateThumb()
        {

            // Arrange
            var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");
            var fi = new FileInfo(source);

            const int width = 500;

            const string target = @"D:\temp\DSC_0187.jpg";

            if (File.Exists(target)) File.Delete(target);
            Assert.IsFalse(File.Exists(target));

            // Act
            ImageTools.GenerateThumb(fi, target, width, width);

            // Assert
            Assert.IsTrue(File.Exists(target));

            var result = ImageTools.GetImageSize(target);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Width > 0);
            Assert.IsTrue(result.Height > 0);
            Assert.IsTrue(result.Width == width);

            //TestHelper.StartFile(target);

        }


        [Test]
        public void TestGenerateWebImage()
        {

            // Arrange
            var source = Path.Combine(TestHelper.GetTestDataFolder(), "DSC_0187.JPG");
            

            const int width = 500;
            const int maxSize = 1000000;

            const string target = @"D:\temp\DSC_0187.jpg";

            if (File.Exists(target)) File.Delete(target);
            Assert.IsFalse(File.Exists(target));

            File.Copy(source, target);

            var fi = new FileInfo(target);

            // Act
            ImageTools.GenerateWebImage(fi, maxSize, width, width);

            // Assert
            Assert.IsTrue(File.Exists(target));

            var result = ImageTools.GetImageSize(target);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Width > 0);
            Assert.IsTrue(result.Height > 0);
            Assert.IsTrue(result.Width == width);

            //TestHelper.StartFile(target);

        }
    }
}