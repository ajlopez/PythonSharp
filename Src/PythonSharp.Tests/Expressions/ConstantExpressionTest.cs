namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using PythonSharp;
    using PythonSharp.Expressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Language;

    [TestClass]
    public class ConstantExpressionTest
    {
        [TestMethod]
        public void EvaluateStringConstantExpressions() 
        {
            ConstantExpression expression = new ConstantExpression("foo");

            Assert.AreEqual("foo", expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateIntegerExpression() 
        {
            ConstantExpression expression = new ConstantExpression(123);

            Assert.AreEqual(123, expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateRealExpression()
        {
            ConstantExpression expression = new ConstantExpression(12.3);

            Assert.AreEqual(12.3, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void EvaluateBooleanConstantExpression()
        {
            ConstantExpression expression = new ConstantExpression(true);

            Assert.AreEqual(true, expression.Evaluate(null));
        }
    }
}

