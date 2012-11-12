namespace PythonSharp.Tests.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Functions;

    [TestClass]
    public class ExecFunctionTests
    {
        private ExecFunction exec;

        [TestInitialize]
        public void Setup()
        {
            this.exec = new ExecFunction();
        }

        [TestMethod]
        public void ExecSimpleText()
        {
            BindingEnvironment environment = new BindingEnvironment();

            this.exec.Apply(environment, new object[] { "a = 1" }, null);

            Assert.AreEqual(1, environment.GetValue("a"));
        }

        [TestMethod]
        public void ExecCommandList()
        {
            BindingEnvironment environment = new BindingEnvironment();

            this.exec.Apply(environment, new object[] { "a = 1; b = 2" }, null);

            Assert.AreEqual(1, environment.GetValue("a"));
            Assert.AreEqual(2, environment.GetValue("b"));
        }

        [TestMethod]
        public void RaiseIfNoArgument()
        {
            try
            {
                this.exec.Apply(null, null, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("exec expected at least 1 arguments, got 0", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseIfFirstArgumentIsNotAString()
        {
            try
            {
                this.exec.Apply(null, new object[] { 0 }, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("exec() arg 1 must be a string, bytes or code object", ex.Message);
            }
        }
    }
}
