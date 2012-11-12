namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    [TestClass]
    public class WhileCommandTests
    {
        [TestMethod]
        public void CreateAndEvaluateSimpleWhileCommand()
        {
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("a", 1);
            IExpression condition = new CompareExpression(ComparisonOperator.Less, new NameExpression("a"), new ConstantExpression(10));
            ICommand body = new SetCommand("a", new BinaryOperatorExpression(new NameExpression("a"), new ConstantExpression(1), BinaryOperator.Add));

            WhileCommand command = new WhileCommand(condition, body);

            command.Execute(environment);

            Assert.AreEqual(condition, command.Condition);
            Assert.AreEqual(body, command.Command);
            Assert.AreEqual(10, environment.GetValue("a"));
        }
    }
}
