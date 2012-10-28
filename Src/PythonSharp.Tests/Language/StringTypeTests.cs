namespace PythonSharp.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;

    [TestClass]
    public class StringTypeTests
    {
        private StringType type;

        [TestInitialize]
        public void Setup()
        {
            this.type = new StringType();
        }

        [TestMethod]
        public void InvokeFind()
        {
            Assert.AreEqual(1, this.type.GetMethod("find").Apply("foo", new object[] { "o" }));
        }

        [TestMethod]
        public void InvokeSingleReplace()
        {
            Assert.AreEqual("sXYZm", this.type.GetMethod("replace").Apply("spam", new object[] { "pa", "XYZ" }));
        }

        [TestMethod]
        public void InvokeMultipleReplace()
        {
            Assert.AreEqual("sXYZmsXYZm", this.type.GetMethod("replace").Apply("spamspam", new object[] { "pa", "XYZ" }));
        }

        [TestMethod]
        public void InvokeNoReplace()
        {
            Assert.AreEqual("spam", this.type.GetMethod("replace").Apply("spam", new object[] { "po", "XYZ" }));
        }

        [TestMethod]
        public void InvokeSingleSplit()
        {
            var result = this.type.GetMethod("split").Apply("spam", new object[] { "a" });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string[]));

            var strings = (string[])result;
            Assert.AreEqual(2, strings.Length);
            Assert.AreEqual("sp", strings[0]);
            Assert.AreEqual("m", strings[1]);
        }

        [TestMethod]
        public void InvokeDoubleSplit()
        {
            var result = this.type.GetMethod("split").Apply("spamspam", new object[] { "a" });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string[]));

            var strings = (string[])result;
            Assert.AreEqual(3, strings.Length);
            Assert.AreEqual("sp", strings[0]);
            Assert.AreEqual("msp", strings[1]);
            Assert.AreEqual("m", strings[2]);
        }

        [TestMethod]
        public void InvokeSplitWithNull()
        {
            var result = this.type.GetMethod("split").Apply("spam", new object[] { null });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string[]));

            var strings = (string[])result;
            Assert.AreEqual(1, strings.Length);
            Assert.AreEqual("spam", strings[0]);
        }

        [TestMethod]
        public void InvokeMultipleSplit()
        {
            var result = this.type.GetMethod("split").Apply("aaa,bbb,ccc,ddd", new object[] { "," });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string[]));

            var strings = (string[])result;
            Assert.AreEqual(4, strings.Length);
            Assert.AreEqual("aaa", strings[0]);
            Assert.AreEqual("bbb", strings[1]);
            Assert.AreEqual("ccc", strings[2]);
            Assert.AreEqual("ddd", strings[3]);
        }

        [TestMethod]
        public void RaiseWhenSplitInvokedWithEmptySeparator()
        {
            try
            {
                this.type.GetMethod("split").Apply("aaa,bbb,ccc,ddd", new object[] { string.Empty });
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValueError));
                Assert.AreEqual("empty separator", ex.Message);
            }
        }
    }
}
