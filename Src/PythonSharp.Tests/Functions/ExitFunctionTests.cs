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
    public class ExitFunctionTests
    {
        [TestMethod]
        public void RaiseMoreThanTwoParameters()
        {
            ExitFunction function = new ExitFunction();

            try
            {
                function.Apply(null, new object[] { 1, 2, 3, 4 }, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("range expected at most 1 arguments, got 4", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseIfSomeParameterIsFloat()
        {
            ExitFunction function = new ExitFunction();

            try
            {
                function.Apply(null, new object[] { 1.0 }, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("'float' object cannot be interpreted as an integer", ex.Message);
            }
        }
    }
}
