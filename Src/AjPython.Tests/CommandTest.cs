namespace AjPython.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AjPython;
    using AjPython.Commands;
    using AjPython.Nodes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void CreateSimpleAssignmentCommand()
        {
            Expression expression = new StringExpression("bar");
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
            Expression expression = new StringExpression("bar");
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
            SimpleAssignmentCommand command = new SimpleAssignmentCommand("foo", new StringExpression("bar"));
            Machine machine = new Machine();

            command.Execute(machine);

            Assert.AreEqual("bar", machine.Environment.GetValue("foo"));
        }

        [TestMethod]
        public void CreatePrintCommand()
        {
            Expression expression = new StringExpression("foo");
            PrintCommand command = new PrintCommand(expression);

            Assert.IsNotNull(command);
            Assert.IsNotNull(command.Expression);
            Assert.AreEqual(expression, command.Expression);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfExpressionIsNullForPrintCommand()
        {
            PrintCommand command = new PrintCommand(null);
        }

        [TestMethod]
        public void ExecutePrintCommand()
        {
            PrintCommand command = new PrintCommand(new StringExpression("bar"));
            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            command.Execute(machine);

            Assert.AreEqual("bar\r\n", writer.ToString());
        }
    }
}
