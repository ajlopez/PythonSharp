namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;
    using PythonSharp.Functions;
    using PythonSharp.Language;

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
            Assert.IsNotNull(machine.Environment.GetValue("eval"));
            Assert.IsInstanceOfType(machine.Environment.GetValue("eval"), typeof(EvalFunction));
            Assert.IsNotNull(machine.Environment.GetValue("exec"));
            Assert.IsInstanceOfType(machine.Environment.GetValue("exec"), typeof(ExecFunction));
            Assert.IsNotNull(machine.Environment.GetValue("dir"));
            Assert.IsInstanceOfType(machine.Environment.GetValue("dir"), typeof(DirFunction));
            Assert.IsNotNull(machine.Environment.GetValue("exit"));
            Assert.IsInstanceOfType(machine.Environment.GetValue("exit"), typeof(ExitFunction));

            var globals = machine.Environment.GetValue("globals");
            Assert.IsNotNull(globals);
            Assert.IsInstanceOfType(globals, typeof(ContextFunction));
            var ctxfunction = (ContextFunction)globals;
            Assert.IsTrue(ctxfunction.IsGlobal);
            Assert.AreEqual("globals", ctxfunction.Name);

            var locals = machine.Environment.GetValue("locals");
            Assert.IsNotNull(globals);
            Assert.IsInstanceOfType(globals, typeof(ContextFunction));
            ctxfunction = (ContextFunction)locals;
            Assert.IsFalse(ctxfunction.IsGlobal);
            Assert.AreEqual("locals", ctxfunction.Name);
        }
    }
}
