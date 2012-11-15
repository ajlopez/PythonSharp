namespace PythonSharp.Tests.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Functions;
    using PythonSharp.Language;

    [TestClass]
    public class ContextFunctionTests
    {
        [TestMethod]
        public void GetLocalContext()
        {
            BindingEnvironment global = new BindingEnvironment();
            BindingEnvironment local = new BindingEnvironment(global);
            ContextFunction function = new ContextFunction("locals", false);
            
            var result = function.Apply(local, null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(local, result);
            Assert.IsFalse(function.IsGlobal);
            Assert.AreEqual("locals", function.Name);
        }

        [TestMethod]
        public void GetGlobalContext()
        {
            BindingEnvironment global = new BindingEnvironment();
            BindingEnvironment local = new BindingEnvironment(global);
            ContextFunction function = new ContextFunction("globals", true);

            var result = function.Apply(local, null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(global, result);
            Assert.IsTrue(function.IsGlobal);
            Assert.AreEqual("globals", function.Name);
        }

        [TestMethod]
        public void RaiseWhenTwoParameters()
        {
            ContextFunction function = new ContextFunction("locals", false);

            try
            {
                function.Apply(null, new object[] { 1, 2 }, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("locals() takes no arguments (2 given)", ex.Message);
            }
        }
    }
}
