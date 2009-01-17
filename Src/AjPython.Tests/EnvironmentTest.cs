namespace AjPython.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnvironmentTest
    {
        [TestMethod]
        public void CanCreate()
        {
            Environment environment = new Environment();

            Assert.IsNotNull(environment);
        }

        [TestMethod]
        public void SetAndGetValue() 
        {
            Environment environment = new Environment();

            environment.SetValue("foo", "bar");

            Assert.AreEqual("bar", environment.GetValue("foo"));
        }

        [TestMethod]
        public void GetNullIfUnknownName()
        {
            Environment environment = new Environment();

            Assert.IsNull(environment.GetValue("foo"));
        }
    }
}

