namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    [TestClass]
    public class ListExpressionTest
    {
        [TestMethod]
        public void CreateListExpression()
        {
            ListExpression expression = new ListExpression(new IExpression[] { });

            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Expressions);
            Assert.AreEqual(0, expression.Expressions.Count);
        }

        [TestMethod]
        public void EvaluateReadOnlyListExpression()
        {
            ListExpression expression = new ListExpression(new List<IExpression>() { new ConstantExpression(1), new ConstantExpression("foo") }, true);

            Assert.IsNotNull(expression.Expressions);
            Assert.AreEqual(2, expression.Expressions.Count);

            object result = expression.Evaluate(new BindingEnvironment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual("foo", list[1]);

            Assert.IsInstanceOfType(result, typeof(ReadOnlyCollection<object>));
        }
    }
}

