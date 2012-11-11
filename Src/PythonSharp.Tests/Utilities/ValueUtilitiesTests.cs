namespace PythonSharp.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Utilities;

    [TestClass]
    public class ValueUtilitiesTests
    {
        [TestMethod]
        public void NullAsString()
        {
            Assert.IsNull(ValueUtilities.AsString(null));
        }

        [TestMethod]
        public void IntegerAsString()
        {
            Assert.AreEqual("1", ValueUtilities.AsString(1));
        }

        [TestMethod]
        public void StringAsString()
        {
            Assert.AreEqual("'hello'", ValueUtilities.AsString("hello"));
        }

        [TestMethod]
        public void StringWithDoubleQuoteAsString()
        {
            Assert.AreEqual("'\"hello\"'", ValueUtilities.AsString("\"hello\""));
        }

        [TestMethod]
        public void StringWithQuoteAsString()
        {
            Assert.AreEqual("\"hello'world\"", ValueUtilities.AsString("hello'world"));
        }

        [TestMethod]
        public void QuoteStringAsString()
        {
            Assert.AreEqual("\"'\"", ValueUtilities.AsString("'"));
        }

        [TestMethod]
        public void StringWithQuoteAndDoubleQuoteAsString()
        {
            Assert.AreEqual("'hello\"\\'world'", ValueUtilities.AsString("hello\"'world"));
        }

        [TestMethod]
        public void NoneAsPrintString()
        {
            Assert.AreEqual("None", ValueUtilities.AsPrintString(null));
        }

        [TestMethod]
        public void IntegerAsPrintString()
        {
            Assert.AreEqual("1", ValueUtilities.AsPrintString(1));
        }

        [TestMethod]
        public void StringAsPrintString()
        {
            Assert.AreEqual("spam", ValueUtilities.AsPrintString("spam"));
        }
    }
}
