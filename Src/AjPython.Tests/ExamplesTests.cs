using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AjPython.Compiler;
using AjPython.Commands;

namespace AjPython.Tests
{
    [TestClass]
    public class ExamplesTests
    {
        [TestMethod]
        [DeploymentItem("Examples/setvar.py")]
        public void EvaluateSetVar()
        {
            Parser parser = new Parser(new StreamReader("setvar.py"));

            ICommand command = parser.CompileCommandList();

            Machine machine = new Machine();

            command.Execute(machine, machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("a"));
        }

        [TestMethod]
        [DeploymentItem("Examples/setvars.py")]
        public void EvaluateSetVars()
        {
            Parser parser = new Parser(new StreamReader("setvars.py"));

            ICommand command = parser.CompileCommandList();

            Machine machine = new Machine();

            command.Execute(machine, machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("one"));
            Assert.AreEqual(2, machine.Environment.GetValue("two"));
        }

        [TestMethod]
        [DeploymentItem("Examples/import.py")]
        [DeploymentItem("Examples/setvar.py")]
        public void EvaluateImport()
        {
            Parser parser = new Parser(new StreamReader("import.py"));

            ICommand command = parser.CompileCommandList();

            Machine machine = new Machine();

            command.Execute(machine, machine.Environment);

            object mod = machine.Environment.GetValue("setvar");

            Assert.IsNotNull(mod);
            Assert.IsInstanceOfType(mod, typeof(BindingEnvironment));

            BindingEnvironment modenv = (BindingEnvironment)mod;

            Assert.AreEqual(1, modenv.GetValue("a"));
        }

        [TestMethod]
        [DeploymentItem("Examples/importfrom.py")]
        [DeploymentItem("Examples/setvars.py")]
        public void EvaluateImportFrom()
        {
            Parser parser = new Parser(new StreamReader("importfrom.py"));

            ICommand command = parser.CompileCommandList();

            Machine machine = new Machine();

            command.Execute(machine, machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("one"));
            Assert.AreEqual(2, machine.Environment.GetValue("two"));
        }
    }
}
