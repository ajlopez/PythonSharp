namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Expressions;

    [TestClass]
    public class IndexedExpressionTests
    {
        [TestMethod]
        public void CreateIndexedExpression()
        {
            IndexedExpression expr = new IndexedExpression(new ConstantExpression("foo"), new ConstantExpression(0));
            Assert.AreEqual("foo", expr.TargetExpression.Evaluate(null));
            Assert.AreEqual(0, expr.IndexExpression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateIndexedExpressionStringInteger()
        {
            IndexedExpression expr = new IndexedExpression(new ConstantExpression("foo"), new ConstantExpression(0));
            Assert.AreEqual("f", expr.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateIndexedExpressionListInteger()
        {
            var list = new List<int>() { 1, 2, 3 };
            IndexedExpression expr = new IndexedExpression(new ConstantExpression(list), new ConstantExpression(1));
            Assert.AreEqual(2, expr.Evaluate(null));
        }
    }
}
