namespace PythonSharp.Tests.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Functions;
    using PythonSharp.Language;
    using PythonSharp.Tests.Classes;

    [TestClass]
    public class IdFunctionTests
    {
        private IFunction id;

        [TestInitialize]
        public void Setup()
        {
            this.id = new IdFunction();
        }

        [TestMethod]
        public void IdString()
        {
            string text = "spam";
            Assert.AreEqual(text.GetHashCode(), this.id.Apply(null, new object[] { text }, null));
        }

        [TestMethod]
        public void IdNull()
        {
            Assert.AreEqual(0, this.id.Apply(null, new object[] { null }, null));
        }

        [TestMethod]
        public void IdObject()
        {
            var person = new Person();
            Assert.AreEqual(person.GetHashCode(), this.id.Apply(null, new object[] { person }, null));
        }

        [TestMethod]
        public void RaiseWhenNullAsArguments()
        {
            try
            {
                this.id.Apply(null, null, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("id() takes exactly one argument (0 given)", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenZeroArguments()
        {
            try
            {
                this.id.Apply(null, new object[] { }, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("id() takes exactly one argument (0 given)", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenTwoArguments()
        {
            try
            {
                this.id.Apply(null, new object[] { 1, 2 }, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("id() takes exactly one argument (2 given)", ex.Message);
            }
        }
    }
}

