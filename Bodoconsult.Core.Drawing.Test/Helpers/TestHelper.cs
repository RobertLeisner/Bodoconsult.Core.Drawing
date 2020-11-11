using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace Bodoconsult.Core.Drawing.Test.Helpers
{
    public static class TestHelper
    {

        private static string testFolder;

        /// <summary>
        /// Get the folder of the current test data
        /// </summary>
        /// <returns></returns>
        public static string GetTestDataFolder()
        {

            if (!string.IsNullOrEmpty(testFolder)) return testFolder;

            var dir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.Parent.Parent;

            testFolder = Path.Combine(dir.FullName, "TestData");

            return testFolder;
        }


        /// <summary>
        /// Start an app by file name
        /// </summary>
        /// <param name="fileName"></param>
        public static void StartFile(string fileName)
        {

            if (!Debugger.IsAttached) return;

            Assert.IsTrue(File.Exists(fileName));

            var p = new Process { StartInfo = new ProcessStartInfo { UseShellExecute = true, FileName = fileName } };

            p.Start();

        }
    }
}
