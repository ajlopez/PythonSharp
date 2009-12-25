namespace AjPython.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython;
    using AjPython.Expressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExpressionTest
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
        public void EvaluateAddExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(1), new ConstantExpression(2), Operator.Add);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(3, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void EvaluateSubtractExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(1), new ConstantExpression(2), Operator.Subtract);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(-1, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void EvaluateMultiplyExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(2), new ConstantExpression(3), Operator.Multiply);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(6, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void EvaluateDivideExpression()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(6), new ConstantExpression(3), Operator.Divide);

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Left);
            Assert.IsNotNull(expression.Right);

            Assert.AreEqual(2, expression.Evaluate(new BindingEnvironment()));
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

            expression.Add(new ConstantExpression(1));
            expression.Add(new ConstantExpression("foo"));

            Assert.IsNotNull(expression.Expressions);
            Assert.AreEqual(2, expression.Expressions.Count);

            object result = expression.Evaluate(new BindingEnvironment());

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
            BinaryExpression expression = new BinaryOperatorExpression(null, new ConstantExpression(3), Operator.Divide);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfRightIsNull()
        {
            BinaryExpression expression = new BinaryOperatorExpression(new ConstantExpression(3), null, Operator.Divide);
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

