namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;
    using PythonSharp.Functions;

    [TestClass]
    public class MachineTest
    {
        [TestMethod]
        public void CreateMachine()
        {
            Machine machine = new Machine();

            Assert.IsNotNull(machine);
            Assert.IsNotNull(machine.Environment);
            Assert.IsNotNull(machine.Input);
            Assert.IsNotNull(machine.Output);
        }

        [TestMethod]
        public void BuiltinFunctions()
        {
            Machine machine = new Machine();

            Assert.IsNotNull(machine.Environment.GetValue("len"));
            Assert.IsInstanceOfType(machine.Environment.GetValue("len"), typeof(LenFunction));
        }
    }
}
