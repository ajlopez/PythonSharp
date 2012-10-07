namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;

    [TestClass]
    public class TypesTests
    {
        [TestMethod]
        public void TypeName()
        {
            Assert.AreEqual("NoneType", Types.GetTypeName(null));
            Assert.AreEqual("int", Types.GetTypeName(1));
            Assert.AreEqual("TypeError", Types.GetTypeName(new TypeError(string.Empty)));
        }
    }
}
