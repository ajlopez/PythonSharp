namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Expressions;
    using PythonSharp.Commands;

    [TestClass]
    public class SetCommandTests
    {
        [TestMethod]
        public void SetVariable()
        {
            IExpression expression = new ConstantExpression(1);
            BindingEnvironment environment = new BindingEnvironment();
            ICommand command = new SetCommand("spam", expression);
            command.Execute(null, environment);

            Assert.AreEqual(1, environment.GetValue("spam"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfNameIsNullForSetCommand()
        {
            IExpression expression = new ConstantExpression("bar");
            SetCommand command = new SetCommand(null, expression);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfExpressionIsNullForSetCommand()
        {
            SetCommand command = new SetCommand("foo", null);
        }

        [TestMethod]
        public void ExecuteSetCommand()
        {
            SetCommand command = new SetCommand("foo", new ConstantExpression("bar"));
            Machine machine = new Machine();

            command.Execute(machine, machine.Environment);

            Assert.AreEqual("bar", machine.Environment.GetValue("foo"));
        }
    }
}
