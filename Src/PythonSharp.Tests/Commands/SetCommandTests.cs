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
            NameExpression variable = new NameExpression("spam");
            BindingEnvironment environment = new BindingEnvironment();
            ICommand command = new SetCommand(variable, expression);
            command.Execute(null, environment);

            Assert.AreEqual(1, environment.GetValue("spam"));
        }
    }
}
