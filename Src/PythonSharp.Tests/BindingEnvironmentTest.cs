namespace PythonSharp.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BindingEnvironmentTest
    {
        [TestMethod]
        public void CanCreate()
        {
            BindingEnvironment environment = new BindingEnvironment();

            Assert.IsNotNull(environment);
            Assert.IsNull(environment.Machine);
            Assert.IsNull(environment.Parent);
            Assert.IsFalse(environment.HasReturnValue());
            Assert.IsNull(environment.GetReturnValue());
        }

        [TestMethod]
        public void SetAndGetValue() 
        {
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("foo", "bar");

            Assert.AreEqual("bar", environment.GetValue("foo"));
        }

        [TestMethod]
        public void SetAndGetReturnValue()
        {
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetReturnValue(1);

            Assert.IsTrue(environment.HasReturnValue());
            Assert.AreEqual(1, environment.GetReturnValue());
        }

        [TestMethod]
        public void HasValue()
        {
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("foo", "bar");
            Assert.IsTrue(environment.HasValue("foo"));
            Assert.IsFalse(environment.HasValue("undefined"));
        }

        [TestMethod]
        public void GetNullIfUnknownName()
        {
            BindingEnvironment environment = new BindingEnvironment();

            Assert.IsNull(environment.GetValue("foo"));
        }

        [TestMethod]
        public void GetValueFromParent()
        {
            BindingEnvironment parent = new BindingEnvironment();
            parent.SetValue("one", 1);
            BindingEnvironment environment = new BindingEnvironment(parent);

            Assert.AreEqual(1, environment.GetValue("one"));
        }

        [TestMethod]
        public void GetMachine()
        {
            Machine machine = new Machine();
            BindingEnvironment environment = new BindingEnvironment(machine);

            Assert.AreEqual(machine, environment.Machine);
        }

        [TestMethod]
        public void GetMachineFromParent()
        {
            Machine machine = new Machine();
            BindingEnvironment parent = new BindingEnvironment(machine);
            parent.SetValue("one", 1);
            BindingEnvironment environment = new BindingEnvironment(parent);

            Assert.AreEqual(machine, environment.Machine);
        }
    }
}

