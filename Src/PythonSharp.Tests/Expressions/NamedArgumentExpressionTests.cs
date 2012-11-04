namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Expressions;

    [TestClass]
    public class NamedArgumentExpressionTests
    {
        [TestMethod]
        public void CreateAndEvaluateNamedArgumentExpression()
        {
            NamedArgumentExpression expression = new NamedArgumentExpression("a", new ConstantExpression(1));

            Assert.AreEqual("a", expression.Name);
            Assert.IsNotNull(expression.Expression);
            Assert.AreEqual(1, expression.Evaluate(null));
        }
    }
}
