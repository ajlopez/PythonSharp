namespace PythonSharp.Tests.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Language;
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
        public void BooleansAsPrintString()
        {
            Assert.AreEqual("True", ValueUtilities.AsPrintString(true));
            Assert.AreEqual("False", ValueUtilities.AsPrintString(false));
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

        [TestMethod]
        public void EmptyListsAsString()
        {
            Assert.AreEqual("[]", ValueUtilities.AsPrintString(new ArrayList()));
            Assert.AreEqual("[]", ValueUtilities.AsPrintString(new List<object>() { }));
            Assert.AreEqual("()", ValueUtilities.AsPrintString(new List<object>().AsReadOnly()));
        }

        [TestMethod]
        public void ListsAsString()
        {
            Assert.AreEqual("[1, 2, 3]", ValueUtilities.AsPrintString(new ArrayList() { 1, 2, 3 }));
            Assert.AreEqual("['a', 'b', 'c']", ValueUtilities.AsPrintString(new List<object>() { "a", "b", "c" }));
            Assert.AreEqual("(1, 2, 'c')", ValueUtilities.AsPrintString((new List<object>() { 1, 2, "c" }).AsReadOnly()));
        }

        [TestMethod]
        public void ClassAsString()
        {
            DefinedClass klass = new DefinedClass("Spam");
            Assert.AreEqual("<class 'Spam'>", ValueUtilities.AsString(klass));
            Assert.AreEqual("<class 'Spam'>", ValueUtilities.AsPrintString(klass));
        }

        [TestMethod]
        public void FunctionAsString()
        {
            DefinedFunction function = new DefinedFunction("foo", null, null);
            Assert.AreEqual("<function foo>", ValueUtilities.AsString(function));
            Assert.AreEqual("<function foo>", ValueUtilities.AsPrintString(function));
        }
    }
}
