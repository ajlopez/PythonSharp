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
            Assert.AreEqual(1, this.type.GetMethod("find").Apply(null, new object[] { "foo", "o" }, null));
        }

        [TestMethod]
        public void InvokeSingleReplace()
        {
            Assert.AreEqual("sXYZm", this.type.GetMethod("replace").Apply(null, new object[] { "spam", "pa", "XYZ" }, null));
        }

        [TestMethod]
        public void HasMethod()
        {
            Assert.IsFalse(this.type.HasMethod("undefined"));
            Assert.IsTrue(this.type.HasMethod("replace"));
        }

        [TestMethod]
        public void GetMethods()
        {
            Assert.IsNull(this.type.GetMethod("undefined"));
            Assert.IsNotNull(this.type.GetMethod("replace"));
        }

        [TestMethod]
        public void InvokeMultipleReplace()
        {
            Assert.AreEqual("sXYZmsXYZm", this.type.GetMethod("replace").Apply(null, new object[] { "spamspam", "pa", "XYZ" }, null));
        }

        [TestMethod]
        public void InvokeNoReplace()
        {
            Assert.AreEqual("spam", this.type.GetMethod("replace").Apply(null, new object[] { "spam", "po", "XYZ" }, null));
        }

        [TestMethod]
        public void InvokeSingleSplit()
        {
            var result = this.type.GetMethod("split").Apply(null, new object[] { "spam", "a" }, null);

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
            var result = this.type.GetMethod("split").Apply(null, new object[] { "spamspam", "a" }, null);

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
            var result = this.type.GetMethod("split").Apply(null, new object[] { "spam", null }, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string[]));

            var strings = (string[])result;
            Assert.AreEqual(1, strings.Length);
            Assert.AreEqual("spam", strings[0]);
        }

        [TestMethod]
        public void InvokeMultipleSplit()
        {
            var result = this.type.GetMethod("split").Apply(null, new object[] { "aaa,bbb,ccc,ddd", "," }, null);

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
        public void InvokeJoin()
        {
            var result = this.type.GetMethod("join").Apply(null, new object[] { ",", new object[] { "1", "2", "3" } }, null);

            Assert.IsNotNull(result);
            Assert.AreEqual("1,2,3", result);
        }

        [TestMethod]
        public void RaiseWhenSplitInvokedWithEmptySeparator()
        {
            try
            {
                this.type.GetMethod("split").Apply(null, new object[] { "aaa,bbb,ccc,ddd", string.Empty }, null);
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
