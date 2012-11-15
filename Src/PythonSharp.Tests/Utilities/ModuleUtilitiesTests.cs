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
        [DeploymentItem("Examples\\setvar.py")]
        public void LoadSimpleModule()
        {
            var context = new BindingEnvironment();
            var module = ModuleUtilities.LoadModule("setvar", context);

            Assert.IsNotNull(module);
            Assert.AreEqual(context, module.GlobalContext);
            Assert.IsNotNull(module.GetValue("a"));
            Assert.AreEqual("setvar module", module.GetValue("__doc__"));
        }

        [TestMethod]
        [DeploymentItem("Modules", "Lib")]
        public void LoadModulesFromLib()
        {
            var context = new BindingEnvironment();
            Assert.IsNotNull(ModuleUtilities.LoadModule("module1", context));
            Assert.IsNotNull(ModuleUtilities.LoadModule("module2", context));
        }

        [TestMethod]
        [DeploymentItem("Examples\\setvar.py")]
        public void LoadCachedModule()
        {
            var context = new BindingEnvironment();
            var module = ModuleUtilities.LoadModule("setvar", context);
            var module2 = ModuleUtilities.LoadModule("setvar", context);

            Assert.IsNotNull(module);
            Assert.AreEqual(module, module2);
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
