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
    public class ReturnCommandTests
    {
        [TestMethod]
        public void CreateAndEvaluateReturnCommand()
        {
            BindingEnvironment environment = new BindingEnvironment();
            ICommand command = new ReturnCommand(new ConstantExpression(1));

            command.Execute(environment);

            Assert.IsTrue(environment.HasReturnValue());
            Assert.AreEqual(1, environment.GetReturnValue());
        }

        [TestMethod]
        public void CreateAndEvaluateReturnCommandWithoutValue()
        {
            BindingEnvironment environment = new BindingEnvironment();
            ICommand command = new ReturnCommand(null);

            command.Execute(environment);

            Assert.IsTrue(environment.HasReturnValue());
            Assert.IsNull(environment.GetReturnValue());
        }
    }
}
