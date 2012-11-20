namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;

    [TestClass]
    public class TryCommandTests
    {
        [TestMethod]
        public void CreateAndExecuteTryCommand()
        {
            BindingEnvironment environment = new BindingEnvironment();
            ICommand body = new SetCommand("a", new ConstantExpression(1));
            TryCommand command = new TryCommand(body);

            command.Execute(environment);

            Assert.AreEqual(1, environment.GetValue("a"));
        }

        [TestMethod]
        public void CreateAndExecuteTryCommandWithFinally()
        {
            BindingEnvironment environment = new BindingEnvironment();
            ICommand body = new SetCommand("a", new ConstantExpression(1));
            ICommand @finally = new SetCommand("b", new ConstantExpression(2));
            TryCommand command = new TryCommand(body);
            command.SetFinally(@finally);

            command.Execute(environment);

            Assert.AreEqual(1, environment.GetValue("a"));
            Assert.AreEqual(2, environment.GetValue("b"));
        }

        [TestMethod]
        public void ExecuteFinallyEvenWhenRaiseException()
        {
            BindingEnvironment environment = new BindingEnvironment();
            ICommand body = new SetCommand("a", new NameExpression("c"));
            ICommand @finally = new SetCommand("b", new ConstantExpression(2));
            TryCommand command = new TryCommand(body);
            command.SetFinally(@finally);

            try
            {
                command.Execute(environment);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(NameError));
                Assert.AreEqual("name 'c' is not defined", ex.Message);
            }

            Assert.IsNull(environment.GetValue("a"));
            Assert.AreEqual(2, environment.GetValue("b"));
        }
    }
}
