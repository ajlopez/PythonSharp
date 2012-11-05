namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Functions;

    [TestClass]
    public class CallExpressionTests
    {
        [TestMethod]
        public void CallLen()
        {
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("len", new LenFunction());
            CallExpression expression = new CallExpression(new NameExpression("len"), new IExpression[] { new ConstantExpression("spam") });

            var result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
            Assert.IsNotNull(expression.TargetExpression);
            Assert.IsNotNull(expression.ArgumentExpressions);
        }

        [TestMethod]
        public void RaiseWhenCallLenWithNullArguments()
        {
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("len", new LenFunction());
            CallExpression expression = new CallExpression(new NameExpression("len"), null);

            try
            {
                expression.Evaluate(environment);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("len() takes exactly one argument (0 given)", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenCallWithNonKeywordArgumentAfterKeywordOne()
        {
            try
            {
                new CallExpression(new NameExpression("foo"), new IExpression[] { new NamedArgumentExpression("a", new ConstantExpression(1)), new ConstantExpression(2) });
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("non-keyword arg after keyword arg", ex.Message);
            }
        }
    }
}
