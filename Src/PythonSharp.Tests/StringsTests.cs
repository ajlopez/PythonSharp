namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringsTests
    {
        [TestMethod]
        public void IsString()
        {
            Assert.IsTrue(Strings.IsString("spam"));
            Assert.IsFalse(Strings.IsString(null));
            Assert.IsFalse(Strings.IsString(123));
        }

        [TestMethod]
        public void Multiply()
        {
            Assert.AreEqual("spamspamspam", Strings.Multiply("spam", 3));
            Assert.AreEqual(string.Empty, Strings.Multiply("spam", 0));
            Assert.AreEqual(string.Empty, Strings.Multiply("spam", -3));
        }
    }
}
