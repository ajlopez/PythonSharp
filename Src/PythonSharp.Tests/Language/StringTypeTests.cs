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
    }
}
