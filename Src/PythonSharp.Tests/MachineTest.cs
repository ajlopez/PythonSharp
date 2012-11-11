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
            Assert.IsNotNull(machine.Environment.GlobalContext);
            Assert.AreEqual(machine.Environment, machine.Environment.GlobalContext);
            Assert.AreEqual(machine, machine.Environment.GetValue("__machine__"));
        }

        [TestMethod]
        public void BuiltinFunctions()
        {
            Machine machine = new Machine();

            Assert.IsNotNull(machine.Environment.GetValue("len"));
            Assert.IsInstanceOfType(machine.Environment.GetValue("len"), typeof(LenFunction));
            Assert.IsNotNull(machine.Environment.GetValue("print"));
            Assert.IsInstanceOfType(machine.Environment.GetValue("print"), typeof(PrintFunction));
            Assert.IsNotNull(machine.Environment.GetValue("range"));
            Assert.IsInstanceOfType(machine.Environment.GetValue("range"), typeof(RangeFunction));
        }
    }
}
