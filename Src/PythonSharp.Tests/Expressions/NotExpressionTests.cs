namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Expressions;

    [TestClass]
    public class NotExpressionTests
    {
        [TestMethod]
        public void CreateNotExpression()
        {
            IExpression trueexpr = new ConstantExpression(true);
            NotExpression expression = new NotExpression(trueexpr);

            Assert.AreEqual(trueexpr, expression.Expression);
        }

        [TestMethod]
        public void EvaluateNotExpression()
        {
            IExpression trueexpr = new ConstantExpression(true);
            IExpression falseexpr = new ConstantExpression(false);

            Assert.IsFalse((bool) (new NotExpression(trueexpr)).Evaluate(null));
            Assert.IsTrue((bool)(new NotExpression(falseexpr)).Evaluate(null));
        }
    }
}
