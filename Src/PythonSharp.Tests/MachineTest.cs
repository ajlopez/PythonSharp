namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;

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
    }
}
