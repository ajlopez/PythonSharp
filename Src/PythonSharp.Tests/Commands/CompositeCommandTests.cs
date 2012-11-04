namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;

    [TestClass]
    public class CompositeCommandTests
    {
        [TestMethod]
        public void ExecuteCompositeCommand()
        {
            SetCommand command1 = new SetCommand("foo", new ConstantExpression("bar"));
            SetCommand command2 = new SetCommand("one", new ConstantExpression(1));

            CompositeCommand command = new CompositeCommand();
            command.AddCommand(command1);
            command.AddCommand(command2);

            Machine machine = new Machine();

            command.Execute(machine.Environment);

            Assert.AreEqual("bar", machine.Environment.GetValue("foo"));
            Assert.AreEqual(1, machine.Environment.GetValue("one"));
            Assert.IsNotNull(command.Commands);
        }

        [TestMethod]
        public void ExecuteCompositeCommandWithReturn()
        {
            SetCommand command1 = new SetCommand("foo", new ConstantExpression("bar"));
            ReturnCommand command2 = new ReturnCommand(new ConstantExpression("spam"));
            SetCommand command3 = new SetCommand("one", new ConstantExpression(1));

            CompositeCommand command = new CompositeCommand();
            command.AddCommand(command1);
            command.AddCommand(command2);
            command.AddCommand(command3);

            Machine machine = new Machine();
            BindingEnvironment environment = new BindingEnvironment(machine.Environment);

            command.Execute(environment);

            Assert.AreEqual("bar", environment.GetValue("foo"));
            Assert.IsNull(environment.GetValue("one"));
            Assert.IsTrue(environment.HasReturnValue());
            Assert.AreEqual("spam", environment.GetReturnValue());
            Assert.IsNotNull(command.Commands);
        }
    }
}
