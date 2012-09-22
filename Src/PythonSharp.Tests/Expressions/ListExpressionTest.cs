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
    public class ListExpressionTest
    {
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
    }
}

