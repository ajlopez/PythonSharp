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

    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void CreateSetCommand()
        {
            IExpression expression = new ConstantExpression("bar");
            SetCommand command = new SetCommand("foo", expression);

            Assert.IsNotNull(command);
            Assert.IsNotNull(command.Target);
            Assert.IsNotNull(command.Expression);

            Assert.AreEqual("foo", command.Target);
            Assert.AreEqual(expression, command.Expression);
        }

        [TestMethod]
        [DeploymentItem("Examples/setvar.py")]
        public void ExecuteImportCommand()
        {
            ImportCommand importcmd = new ImportCommand("setvar");

            Machine machine = new Machine();

            importcmd.Execute(machine.Environment);

            object setvar = machine.Environment.GetValue("setvar");

            Assert.IsNotNull(setvar);
            Assert.IsInstanceOfType(setvar, typeof(BindingEnvironment));

            BindingEnvironment setvarenv = (BindingEnvironment)setvar;

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
    }
}
