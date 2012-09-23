namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    [TestClass]
    public class SliceExpressionTests
    {
        [TestMethod]
        public void EvaluateSliceExpression()
        {
            SliceExpression expression = new SliceExpression(new ConstantExpression(1), new ConstantExpression(2));

            Assert.IsNotNull(expression.BeginExpression);
            Assert.IsNotNull(expression.EndExpression);

            var result = expression.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Slice));

            Slice slice = (Slice)result;

            Assert.IsTrue(slice.Begin.HasValue);
            Assert.IsTrue(slice.End.HasValue);
            Assert.AreEqual(1, slice.Begin.Value);
            Assert.AreEqual(2, slice.End.Value);
        }
    }
}
