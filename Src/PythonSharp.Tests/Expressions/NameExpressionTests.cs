namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Expressions;
    using PythonSharp.Exceptions;

    [TestClass]
    public class NameExpressionTests
    {
        [TestMethod]
        public void EvaluateNameExpression()
        {
            NameExpression expression = new NameExpression("foo");

            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("foo", "bar");

            Assert.AreEqual("bar", expression.Evaluate(environment));
        }

        [TestMethod]
        public void RaiseIfNameIsUndefined()
        {
            NameExpression expression = new NameExpression("foo");

            BindingEnvironment environment = new BindingEnvironment();

            try
            {
                expression.Evaluate(environment);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(NameError));
                Assert.AreEqual("name 'foo' is not defined", ex.Message);
            }
        }
    }
}
