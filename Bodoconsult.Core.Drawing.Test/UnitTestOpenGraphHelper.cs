using System.IO;
using Bodoconsult.Core.Drawing.Test.Helpers;
using NUnit.Framework;

namespace Bodoconsult.Core.Drawing.Test
{
    public class UnitTestOpenGraphHelper
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
        public void TestSave()
        {
            // Arrange
            var target = Path.Combine(TestHelper.TargetFolder, OpenGraphHelper.OpenGraphFileName);

            if(File.Exists(target)) File.Delete(target);
            Assert.IsFalse(File.Exists(target));

            OpenGraphHelper.SourceFile = Path.Combine(TestHelper.GetTestDataFolder(), "OpenGraphBasis.png");

            // Act
            OpenGraphHelper.Save("Test", "Test description", target);

            // Assert
            Assert.IsTrue(File.Exists(target));

        }
    }
}