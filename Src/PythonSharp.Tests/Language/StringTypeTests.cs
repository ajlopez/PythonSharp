namespace PythonSharp.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
