namespace PythonSharp.Tests.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Functions;
    using PythonSharp.Tests.Classes;

    [TestClass]
    public class DirFunctionTests
    {
        private DirFunction dir;

        [TestInitialize]
        public void Setup()
        {
            this.dir = new DirFunction();
        }

        [TestMethod]
        public void GetMachineNames()
        {
            Machine machine = new Machine();
            var result = this.dir.Apply(machine.Environment, new object[] { machine.Environment }, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<string>));

            var names = (IList<string>)result;

            Assert.AreNotEqual(0, names.Count);
            Assert.IsTrue(names.Contains("len"));
            Assert.IsTrue(names.Contains("exec"));
            Assert.IsTrue(names.Contains("eval"));

            var previous = string.Empty;

            foreach (var name in names)
            {
                Assert.IsTrue(previous.CompareTo(name) < 0);
                previous = name;
            }
        }

        [TestMethod]
        public void GetCurrentContextNames()
        {
            BindingEnvironment context = new BindingEnvironment();

            context.SetValue("zero", 0);
            context.SetValue("one", 1);
            context.SetValue("two", 2);
            context.SetValue("three", 3);

            var result = this.dir.Apply(context, null, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<string>));

            var names = (IList<string>)result;

            Assert.AreNotEqual(0, names.Count);
            Assert.IsTrue(names.Contains("zero"));
            Assert.IsTrue(names.Contains("one"));
            Assert.IsTrue(names.Contains("two"));
            Assert.IsTrue(names.Contains("three"));

            var previous = string.Empty;

            foreach (var name in names)
            {
                Assert.IsTrue(previous.CompareTo(name) < 0);
                previous = name;
            }
        }

        [TestMethod]
        public void RaiseWhenTwoArguments()
        {
            try
            {
                this.dir.Apply(null, new object[] { 1, 2 }, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("dir expected at most 1 arguments, got 2", ex.Message);
            }
        }

        [TestMethod]
        public void NativeObject()
        {
            var person = new Person();
            var result = this.dir.Apply(null, new object[] { person }, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<string>));

            var names = (IList<string>)result;

            Assert.IsTrue(names.Contains("FirstName"));
            Assert.IsTrue(names.Contains("LastName"));
            Assert.IsTrue(names.Contains("GetName"));
            Assert.IsTrue(names.Contains("NameEvent"));
        }
    }
}
