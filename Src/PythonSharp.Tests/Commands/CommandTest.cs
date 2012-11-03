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
        public void CreatePrintCommand()
        {
            IList<IExpression> expressions = new IExpression[] { new ConstantExpression("foo") };
            PrintCommand command = new PrintCommand(expressions);

            Assert.IsNotNull(command);
            Assert.IsNotNull(command.Expressions);
            Assert.AreEqual(expressions, command.Expressions);
        }

        [TestMethod]
        public void ExecutePrintCommand()
        {
            PrintCommand command = new PrintCommand(new IExpression[] { new ConstantExpression("bar") });
            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            command.Execute(machine, machine.Environment);

            Assert.AreEqual("bar\r\n", writer.ToString());
        }

        [TestMethod]
        public void ExecutePrintCommandWithTwoValues()
        {
            PrintCommand command = new PrintCommand(new IExpression[] { new ConstantExpression("bar"), new ConstantExpression("foo") });
            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            command.Execute(machine, machine.Environment);

            Assert.AreEqual("bar foo\r\n", writer.ToString());
        }

        [TestMethod]
        public void ExecuteCompositeCommand()
        {
            SetCommand command1 = new SetCommand("foo", new ConstantExpression("bar"));
            SetCommand command2 = new SetCommand("one", new ConstantExpression(1));

            CompositeCommand command = new CompositeCommand();
            command.AddCommand(command1);
            command.AddCommand(command2);

            Machine machine = new Machine();

            command.Execute(machine, machine.Environment);

            Assert.AreEqual("bar", machine.Environment.GetValue("foo"));
            Assert.AreEqual(1, machine.Environment.GetValue("one"));
            Assert.IsNotNull(command.Commands);
        }

        [TestMethod]
        [DeploymentItem("Examples/setvar.py")]
        public void ExecuteImportCommand()
        {
            ImportCommand importcmd = new ImportCommand("setvar");

            Machine machine = new Machine();

            importcmd.Execute(machine, machine.Environment);

            object setvar = machine.Environment.GetValue("setvar");

            Assert.IsNotNull(setvar);
            Assert.IsInstanceOfType(setvar, typeof(BindingEnvironment));

            BindingEnvironment setvarenv = (BindingEnvironment)setvar;

            object var = setvarenv.GetValue("a");

            Assert.IsNotNull(var);
            Assert.AreEqual(1, var);
        }

        [TestMethod]
        [DeploymentItem("Examples/setvars.py")]
        public void ExecuteImportFromCommand()
        {
            ImportFromCommand importcmd = new ImportFromCommand("setvars", new string[] { "one", "two" });

            Machine machine = new Machine();

            importcmd.Execute(machine, machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("one"));
            Assert.AreEqual(2, machine.Environment.GetValue("two"));
        }

        [TestMethod]
        public void ExecuteIfCommandWithTrueCondition()
        {
            IfCommand ifcmd = new IfCommand(new ConstantExpression(true), new SetCommand("one", new ConstantExpression(1)));
            Machine machine = new Machine();

            ifcmd.Execute(machine, machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("one"));
        }

        [TestMethod]
        public void ExecuteIfCommandWithFalseCondition()
        {
            IfCommand ifcmd = new IfCommand(new ConstantExpression(false), new SetCommand("one", new ConstantExpression(1)));
            Machine machine = new Machine();

            ifcmd.Execute(machine, machine.Environment);

            Assert.IsNull(machine.Environment.GetValue("one"));
        }
    }
}
