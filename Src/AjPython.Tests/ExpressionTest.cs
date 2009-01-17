namespace AjPython.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython;
    using AjPython.Nodes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExpressionTest
    {
        [TestMethod]
        public void EvaluateStringExpression() 
        {
            StringExpression expression = new StringExpression("foo");

            Assert.AreEqual("foo", expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void EvaluateIntegerExpression() 
        {
            IntegerExpression expression = new IntegerExpression(123);

            Assert.AreEqual(123, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void EvaluateRealExpression()
        {
            RealExpression expression = new RealExpression(12.3);

            Assert.AreEqual(12.3, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void EvaluateBooleanExpression()
        {
            BooleanExpression expression = new BooleanExpression(true);

            Assert.AreEqual(true, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void EvaluateNameExpression() 
        {
            NameExpression expression = new NameExpression("foo");

            Environment environment = new Environment();

            environment.SetValue("foo", "bar");

            Assert.AreEqual("bar", expression.Evaluate(environment));
        }

        [TestMethod]
        public void EvaluateAddExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new IntegerExpression(1), new IntegerExpression(2), Operator.Add);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(3, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void EvaluateSubtractExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new IntegerExpression(1), new IntegerExpression(2), Operator.Subtract);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(-1, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void EvaluateMultiplyExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new IntegerExpression(2), new IntegerExpression(3), Operator.Multiply);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(6, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void EvaluateDivideExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new IntegerExpression(6), new IntegerExpression(3), Operator.Divide);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(2, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void CreateListExpression()
        {
            ListExpression expression = new ListExpression();

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Expressions);
            Assert.AreEqual(0, expression.Expressions.Count);
        }

        [TestMethod]
        public void EvaluateListExpression()
        {
            ListExpression expression = new ListExpression();

            expression.Add(new IntegerExpression(1));
            expression.Add(new StringExpression("foo"));

            Assert.IsNotNull(expression.Expressions);
            Assert.AreEqual(2, expression.Expressions.Count);

            object result = expression.Evaluate(new Environment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual("foo", list[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfLeftIsNull()
        {
            BinaryExpression expression = new BinaryOperatorExpression(null, new IntegerExpression(3), Operator.Divide);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfRightIsNull()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new IntegerExpression(3), null, Operator.Divide);
        }
    }
}

