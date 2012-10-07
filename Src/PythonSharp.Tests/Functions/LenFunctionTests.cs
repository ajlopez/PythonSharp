namespace PythonSharp.Tests.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Functions;
    using PythonSharp.Language;

    [TestClass]
    public class LenFunctionTests
    {
        private IFunction len;

        [TestInitialize]
        public void Setup()
        {
            this.len = new LenFunction();
        }

        [TestMethod]
        public void LenString()
        {
            Assert.AreEqual(4, this.len.Apply(new object[] { "spam" }));
            Assert.AreEqual(0, this.len.Apply(new object[] { string.Empty }));
        }

        [TestMethod]
        public void LenList()
        {
            Assert.AreEqual(3, this.len.Apply(new object[] { new object[] { 1, 2, 3 } }));
            Assert.AreEqual(2, this.len.Apply(new object[] { new List<object>() { 1, 2 } }));
        }

        [TestMethod]
        public void LenDictionary()
        {
            IDictionary dictionary = new Hashtable() { { "one", 1 }, { "two", 2 } };
            Assert.AreEqual(2, this.len.Apply(new object[] { dictionary }));
        }

        [TestMethod]
        public void RaiseWhenNullAsArguments()
        {
            try
            {
                this.len.Apply(null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
                Assert.AreEqual("len() takes exactly one argument (0 given)", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenZeroArguments()
        {
            try
            {
                this.len.Apply(new object[] { });
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
                Assert.AreEqual("len() takes exactly one argument (0 given)", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenTwoArguments()
        {
            try
            {
                this.len.Apply(new object[] { 1, 2 });
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
                Assert.AreEqual("len() takes exactly one argument (2 given)", ex.Message);
            }
        }
    }
}

