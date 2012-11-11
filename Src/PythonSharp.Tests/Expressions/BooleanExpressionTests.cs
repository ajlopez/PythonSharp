namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    [TestClass]
    public class BooleanExpressionTests
    {
        [TestMethod]
        public void EvaluateOr()
        {
            IExpression trueexpr = new ConstantExpression(true);
            IExpression falseexpr = new ConstantExpression(false);

            Assert.IsTrue((bool)(new BooleanExpression(trueexpr, trueexpr, BooleanOperator.Or)).Evaluate(null));
            Assert.IsTrue((bool)(new BooleanExpression(trueexpr, falseexpr, BooleanOperator.Or)).Evaluate(null));
            Assert.IsTrue((bool)(new BooleanExpression(falseexpr, trueexpr, BooleanOperator.Or)).Evaluate(null));
            Assert.IsFalse((bool)(new BooleanExpression(falseexpr, falseexpr, BooleanOperator.Or)).Evaluate(null));
        }

        [TestMethod]
        public void OrIsShortcircuit()
        {
            IExpression trueexpr = new ConstantExpression(true);
            IExpression dividebyzero = new BinaryOperatorExpression(new ConstantExpression(1), new ConstantExpression(0), BinaryOperator.Divide);

            Assert.IsTrue((bool)(new BooleanExpression(trueexpr, dividebyzero, BooleanOperator.Or)).Evaluate(null));
        }

        [TestMethod]
        public void EvaluateAnd()
        {
            IExpression trueexpr = new ConstantExpression(true);
            IExpression falseexpr = new ConstantExpression(false);

            Assert.IsTrue((bool)(new BooleanExpression(trueexpr, trueexpr, BooleanOperator.And)).Evaluate(null));
            Assert.IsFalse((bool)(new BooleanExpression(trueexpr, falseexpr, BooleanOperator.And)).Evaluate(null));
            Assert.IsFalse((bool)(new BooleanExpression(falseexpr, trueexpr, BooleanOperator.And)).Evaluate(null));
            Assert.IsFalse((bool)(new BooleanExpression(falseexpr, falseexpr, BooleanOperator.And)).Evaluate(null));
        }

        [TestMethod]
        public void AndIsShortcircuit()
        {
            IExpression falseexpr = new ConstantExpression(false);
            IExpression dividebyzero = new BinaryOperatorExpression(new ConstantExpression(1), new ConstantExpression(0), BinaryOperator.Divide);

            Assert.IsFalse((bool)(new BooleanExpression(falseexpr, dividebyzero, BooleanOperator.And)).Evaluate(null));
        }
    }
}
