namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Compiler;
    using PythonSharp.Language;

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

            command.Execute(machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("a"));
        }

        [TestMethod]
        [DeploymentItem("Examples/setvars.py")]
        public void EvaluateSetVars()
        {
            Parser parser = new Parser(new StreamReader("setvars.py"));

            ICommand command = parser.CompileCommandList();

            Machine machine = new Machine();

            command.Execute(machine.Environment);

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

            command.Execute(machine.Environment);

            object mod = machine.Environment.GetValue("setvar");

            Assert.IsNotNull(mod);
            Assert.IsInstanceOfType(mod, typeof(IValues));

            IValues modenv = (IValues)mod;

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

            command.Execute(machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("one"));
            Assert.AreEqual(2, machine.Environment.GetValue("two"));
        }

        [TestMethod]
        [DeploymentItem("Examples/defif.py")]
        public void CompileDefIf()
        {
            Parser parser = new Parser(new StreamReader("defif.py"));

            ICommand command = parser.CompileCommandList();
        }

        [TestMethod]
        [DeploymentItem("Examples/httpserver.py")]
        public void CompileHttpServer()
        {
            Parser parser = new Parser(new StreamReader("httpserver.py"));

            ICommand command = parser.CompileCommandList();
        }
    }
}
