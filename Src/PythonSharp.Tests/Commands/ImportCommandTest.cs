namespace PythonSharp.Tests.Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;
    using PythonSharp.Language;
    using System;

    [TestClass]
    public class ImportCommandTest
    {
        [TestMethod]
        [DeploymentItem("Examples/setvar.py")]
        public void ExecuteImportCommand()
        {
            ImportCommand importcmd = new ImportCommand("setvar");

            Machine machine = new Machine();

            importcmd.Execute(machine.Environment);

            object setvar = machine.Environment.GetValue("setvar");

            Assert.IsNotNull(setvar);
            Assert.IsInstanceOfType(setvar, typeof(IValues));

            IValues setvarenv = (IValues)setvar;

            object var = setvarenv.GetValue("a");

            Assert.IsNotNull(var);
            Assert.AreEqual(1, var);

            object doc = setvarenv.GetValue("__doc__");

            Assert.IsNotNull(doc);
            Assert.AreEqual("setvar module", doc);
        }

        [TestMethod]
        [DeploymentItem("Examples/setvars.py")]
        public void ExecuteImportFromCommand()
        {
            ImportFromCommand importcmd = new ImportFromCommand("setvars", new string[] { "one", "two" });

            Machine machine = new Machine();

            importcmd.Execute(machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("one"));
            Assert.AreEqual(2, machine.Environment.GetValue("two"));
        }

        [TestMethod]
        [DeploymentItem("Examples/setvars.py")]
        public void ImportModuleWithEmptyDocString()
        {
            ImportCommand importcmd = new ImportCommand("setvars");

            Machine machine = new Machine();

            importcmd.Execute(machine.Environment);

            var setvar = (IValues)machine.Environment.GetValue("setvars");

            Assert.IsNull(setvar.GetValue("__doc__"));
            Assert.IsTrue(setvar.HasValue("__doc__"));
        }

        [TestMethod]
        public void ImportFileTypeFromSystemIONamespace()
        {
            ImportFromCommand importcmd = new ImportFromCommand("System.IO", new string[] { "File" });

            Machine machine = new Machine();

            importcmd.Execute(machine.Environment);

            var result = machine.Environment.GetValue("File");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Type));
        }

        [TestMethod]
        public void ImportFromSystemIONamespace()
        {
            ImportFromCommand importcmd = new ImportFromCommand("System.IO");

            Machine machine = new Machine();

            importcmd.Execute(machine.Environment);

            var result = machine.Environment.GetValue("File");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Type));

            Assert.IsNotNull(machine.Environment.GetValue("FileInfo"));
            Assert.IsNotNull(machine.Environment.GetValue("Directory"));
            Assert.IsNotNull(machine.Environment.GetValue("DirectoryInfo"));
        }

        [TestMethod]
        public void ImportSystemIONamespace()
        {
            ImportCommand importcmd = new ImportCommand("System.IO");

            Machine machine = new Machine();

            importcmd.Execute(machine.Environment);

            var result = machine.Environment.GetValue("System");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Module));

            var ioresult = ((Module)result).GetValue("IO");
            Assert.IsNotNull(ioresult);
            Assert.IsInstanceOfType(ioresult, typeof(Module));

            var module = (Module)ioresult;

            Assert.IsNotNull(module.GetValue("FileInfo"));
            Assert.IsNotNull(module.GetValue("Directory"));
            Assert.IsNotNull(module.GetValue("DirectoryInfo"));
        }
    }
}
