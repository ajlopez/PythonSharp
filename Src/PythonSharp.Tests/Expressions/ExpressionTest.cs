namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    [TestClass]
    public class ExpressionTest
    {
        [TestMethod]
        public void EvaluateCompareExpression()
        {
            CompareExpression expression = new CompareExpression(ComparisonOperator.Equal, new ConstantExpression(1), new ConstantExpression(2));
            var result = expression.Evaluate(null);

            Assert.IsInstanceOfType(result, typeof(bool));
            Assert.IsFalse((bool)result);
            Assert.AreEqual(ComparisonOperator.Equal, expression.Operation);
        }

        [TestMethod]
        public void EvaluateNameExpression() 
        {
            NameExpression expression = new NameExpression("foo");

            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("foo", "bar");

            Assert.AreEqual("bar", expression.Evaluate(environment));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "NameError: name 'foo' is not defined")]
        public void RaiseIfNameIsUndefined()
        {
            NameExpression expression = new NameExpression("foo");

            BindingEnvironment environment = new BindingEnvironment();

            expression.Evaluate(environment);
        }

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
        }
    }
}

