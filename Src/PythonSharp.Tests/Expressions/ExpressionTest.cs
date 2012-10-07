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
    }
}

