namespace AjPython.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AjPython;
    using AjPython.Commands;
    using AjPython.Expressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void CreateSimpleAssignmentCommand()
        {
            IExpression expression = new ConstantExpression("bar");
            SimpleAssignmentCommand command = new SimpleAssignmentCommand("foo", expression);

            Assert.IsNotNull(command);
            Assert.IsNotNull(command.Name);
            Assert.IsNotNull(command.Expression);

            Assert.AreEqual("foo", command.Name);
            Assert.AreEqual(expression, command.Expression);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfNameIsNullForSimpleAssignmentCommand() 
        {
            IExpression expression = new ConstantExpression("bar");
            SimpleAssignmentCommand command = new SimpleAssignmentCommand(null, expression);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfExpressionIsNullForSimpleAssignmentCommand()
        {
            SimpleAssignmentCommand command = new SimpleAssignmentCommand("foo", null);
        }

        [TestMethod]
        public void ExecuteSimpleAssignmentCommand()
        {
            SimpleAssignmentCommand command = new SimpleAssignmentCommand("foo", new ConstantExpression("bar"));
            Machine machine = new Machine();

            command.Execute(machine, machine.Environment);

            Assert.AreEqual("bar", machine.Environment.GetValue("foo"));
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
        public void ExecuteCompositeCommand()
        {
            SimpleAssignmentCommand command1 = new SimpleAssignmentCommand("foo", new ConstantExpression("bar"));
            SimpleAssignmentCommand command2 = new SimpleAssignmentCommand("one", new ConstantExpression(1));

            CompositeCommand command = new CompositeCommand();
            command.AddCommand(command1);
            command.AddCommand(command2);

            Machine machine = new Machine();

            command.Execute(machine, machine.Environment);

            Assert.AreEqual("bar", machine.Environment.GetValue("foo"));
            Assert.AreEqual(1, machine.Environment.GetValue("one"));
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
            IfCommand ifcmd = new IfCommand(new ConstantExpression(true), new SimpleAssignmentCommand("one", new ConstantExpression(1)));
            Machine machine = new Machine();

            ifcmd.Execute(machine, machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("one"));
        }

        [TestMethod]
        public void ExecuteIfCommandWithFalseCondition()
        {
            IfCommand ifcmd = new IfCommand(new ConstantExpression(false), new SimpleAssignmentCommand("one", new ConstantExpression(1)));
            Machine machine = new Machine();

            ifcmd.Execute(machine, machine.Environment);

            Assert.IsNull(machine.Environment.GetValue("one"));
        }
    }
}
