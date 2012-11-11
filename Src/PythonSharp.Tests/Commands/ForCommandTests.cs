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
    using PythonSharp.Exceptions;

    [TestClass]
    public class ForCommandTests
    {
        [TestMethod]
        public void ExecuteSimpleFor()
        {
            ICommand body = new SetCommand("b", new BinaryOperatorExpression(new NameExpression("a"), new NameExpression("b"), BinaryOperator.Add));
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("b", 0);

            ForCommand command = new ForCommand("a", new ConstantExpression(new object[] { 1, 2, 3 }), body);

            command.Execute(environment);

            Assert.AreEqual(6, environment.GetValue("b"));
        }

        [TestMethod]
        public void ExecuteSimpleForOnEmptyList()
        {
            ICommand body = new SetCommand("b", new BinaryOperatorExpression(new NameExpression("a"), new NameExpression("b"), BinaryOperator.Add));
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("b", 0);

            ForCommand command = new ForCommand("a", new ConstantExpression(new object[] { }), body);

            command.Execute(environment);

            Assert.AreEqual(0, environment.GetValue("b"));
        }

        [TestMethod]
        public void RaiseWhenForOverNone()
        {
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("b", 0);

            ForCommand command = new ForCommand("a", new ConstantExpression(null), null);

            try
            {
                command.Execute(environment);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("'NoneType' object is not iterable", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenForOverAnInteger()
        {
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("b", 0);

            ForCommand command = new ForCommand("a", new ConstantExpression(1), null);

            try
            {
                command.Execute(environment);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("'int' object is not iterable", ex.Message);
            }
        }
    }
}
