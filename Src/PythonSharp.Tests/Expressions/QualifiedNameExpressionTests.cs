namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    [TestClass]
    public class QualifiedNameExpressionTests
    {
        [TestMethod]
        public void EvaluateQualifiedNameExpression()
        {
            QualifiedNameExpression expression = new QualifiedNameExpression("module", "foo");
            BindingEnvironment environment = new BindingEnvironment();
            BindingEnvironment modenv = new BindingEnvironment();

            modenv.SetValue("foo", "bar");
            environment.SetValue("module", modenv);

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("bar", result);
            Assert.AreEqual("module", expression.ModuleName);
            Assert.AreEqual("foo", expression.Name);
        }

        [TestMethod]
        public void RaiseWhenEvaluateQualifiedUndefinedNameExpression()
        {
            QualifiedNameExpression expression = new QualifiedNameExpression("module", "spam");
            BindingEnvironment environment = new BindingEnvironment();
            BindingEnvironment modenv = new BindingEnvironment();

            environment.SetValue("module", modenv);

            try
            {
                expression.Evaluate(environment);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(AttributeError));
                Assert.AreEqual("'module' object has no attribute 'spam'", ex.Message);
            }
        }
    }
}

