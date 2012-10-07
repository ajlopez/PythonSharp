namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Expressions;

    [TestClass]
    public class SlicedExpressionTests
    {
        [TestMethod]
        public void EvaluateStringSlicedExpression()
        {
            SlicedExpression expression = CreateSlicedExpression("spam", 1, 3);
            Assert.AreEqual("pa", expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateStringSlicedExpressionWithNullBegin()
        {
            SlicedExpression expression = CreateSlicedExpression("spam", null, 2);
            Assert.AreEqual("sp", expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateStringSlicedExpressionWithNullBeginAndEnd()
        {
            SlicedExpression expression = CreateSlicedExpression("spam", null, null);
            Assert.AreEqual("spam", expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateStringSlicedExpressionWithNullEnd()
        {
            SlicedExpression expression = CreateSlicedExpression("spam", 1, null);
            Assert.AreEqual("pam", expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateStringSlicedExpressionBeyondEnd()
        {
            SlicedExpression expression = CreateSlicedExpression("spam", 1, 10);
            Assert.AreEqual("pam", expression.Evaluate(null));
        }

        [TestMethod]
        public void EvaluateListSlicedExpression()
        {
            IList list = new List<object>() { 1, 2, 3, 4 };
            SlicedExpression expression = CreateSlicedExpression(list, 1, 3);
            var result = expression.Evaluate(null);
            Assert.IsInstanceOfType(result, typeof(IList));
            IList lresult = (IList)result;
            Assert.AreEqual(2, lresult.Count);
            Assert.AreEqual(2, lresult[0]);
            Assert.AreEqual(3, lresult[1]);
        }

        [TestMethod]
        public void EvaluateListSlicedExpressionWithNullEnd()
        {
            IList list = new List<object>() { 1, 2, 3, 4 };
            SlicedExpression expression = CreateSlicedExpression(list, 1, null);
            var result = expression.Evaluate(null);
            Assert.IsInstanceOfType(result, typeof(IList));
            IList lresult = (IList)result;
            Assert.AreEqual(3, lresult.Count);
            Assert.AreEqual(2, lresult[0]);
            Assert.AreEqual(3, lresult[1]);
            Assert.AreEqual(4, lresult[2]);
        }

        [TestMethod]
        public void EvaluateListSlicedExpressionWithNullBegin()
        {
            IList list = new List<object>() { 1, 2, 3, 4 };
            SlicedExpression expression = CreateSlicedExpression(list, null, 2);
            var result = expression.Evaluate(null);
            Assert.IsInstanceOfType(result, typeof(IList));
            IList lresult = (IList)result;
            Assert.AreEqual(2, lresult.Count);
            Assert.AreEqual(1, lresult[0]);
            Assert.AreEqual(2, lresult[1]);
        }

        [TestMethod]
        public void EvaluateListSlicedExpressionWithNullBeginAndEnd()
        {
            IList list = new List<object>() { 1, 2, 3, 4 };
            SlicedExpression expression = CreateSlicedExpression(list, null, null);
            var result = expression.Evaluate(null);
            Assert.IsInstanceOfType(result, typeof(IList));
            IList lresult = (IList)result;
            Assert.AreEqual(4, lresult.Count);
            Assert.AreEqual(1, lresult[0]);
            Assert.AreEqual(2, lresult[1]);
            Assert.AreEqual(3, lresult[2]);
            Assert.AreEqual(4, lresult[3]);
        }

        [TestMethod]
        public void EvaluateListSlicedExpressionBeyondEnd()
        {
            IList list = new List<object>() { 1, 2, 3, 4 };
            SlicedExpression expression = CreateSlicedExpression(list, 3, 10);
            var result = expression.Evaluate(null);
            Assert.IsInstanceOfType(result, typeof(IList));
            IList lresult = (IList)result;
            Assert.AreEqual(1, lresult.Count);
            Assert.AreEqual(4, lresult[0]);
        }

        private static SlicedExpression CreateSlicedExpression(object target, int? begin, int? end)
        {
            IExpression targetExpression = new ConstantExpression(target);
            SliceExpression sliceExpression = new SliceExpression(begin.HasValue ? new ConstantExpression(begin.Value) : null, end.HasValue ? new ConstantExpression(end.Value) : null);
            return new SlicedExpression(targetExpression, sliceExpression);
        }
    }
}
