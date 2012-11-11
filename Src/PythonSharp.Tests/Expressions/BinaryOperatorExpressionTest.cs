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
    public class BinaryOperatorExpressionTest
    {
        [TestMethod]
        public void EvaluateAddExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(1), new ConstantExpression(2), BinaryOperator.Add);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(3, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void EvaluateSubtractExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(1), new ConstantExpression(2), BinaryOperator.Subtract);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(-1, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void EvaluateMultiplyExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(2), new ConstantExpression(3), BinaryOperator.Multiply);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(6, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void EvaluateMultiplyExpressionWithLeftString()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression("spam"), new ConstantExpression(3), BinaryOperator.Multiply);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual("spamspamspam", expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void EvaluateDivideExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(6), new ConstantExpression(3), BinaryOperator.Divide);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(2, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void EvaluateDivideToFloatExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(4), new ConstantExpression(5), BinaryOperator.Divide);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(0.8, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfLeftIsNull()
        {
            BinaryExpression expression = new BinaryOperatorExpression(null, new ConstantExpression(3), BinaryOperator.Divide);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfRightIsNull()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(3), null, BinaryOperator.Divide);
        }
    }
}

