namespace PythonSharp.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Language;

    [TestClass]
    public class ModuleTests
    {
        [TestMethod]
        public void CreateModule()
        {
            Module module = new Module(null);

            Assert.IsNull(module.GlobalContext);
            Assert.IsNull(module.GetValue("foo"));
            Assert.IsFalse(module.HasValue("foo"));

            var names = module.GetNames();

            Assert.IsNotNull(names);
            Assert.AreEqual(0, names.Count);
        }

        [TestMethod]
        public void CreateModuleWithGlobalContext()
        {
            Module global = new Module(null);
            Module module = new Module(global);

            Assert.IsNotNull(module.GlobalContext);
            Assert.AreEqual(global, module.GlobalContext);
        }
    }
}
