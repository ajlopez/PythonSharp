namespace PythonSharp.Tests.Utilities
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Utilities;

    [TestClass]
    public class ModuleUtilitiesTests
    {
        [TestMethod]
        public void UnknownModule()
        {
            Assert.IsNull(ModuleUtilities.ModuleFileName("spam"));
        }

        [TestMethod]
        [DeploymentItem("Modules")]
        public void ModuleFilename()
        {
            var result = ModuleUtilities.ModuleFileName("module1");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.EndsWith("module1.py"));
        }

        [TestMethod]
        [DeploymentItem("Modules")]
        public void ModuleFilenameInitInFolder()
        {
            var result = ModuleUtilities.ModuleFileName("module2");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.EndsWith("__init__.py"));
        }
    }
}
