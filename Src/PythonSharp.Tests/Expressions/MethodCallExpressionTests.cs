namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Expressions;

    [TestClass]
    public class MethodCallExpressionTests
    {
        [TestMethod]
        public void InvokeStringFind()
        {
            MethodCallExpression expression = new MethodCallExpression(new ConstantExpression("spam"), "find", new IExpression[] { new ConstantExpression("pa") });
            Assert.AreEqual(1, expression.Evaluate(null));
            Assert.IsNotNull(expression.TargetExpression);
            Assert.IsNotNull(expression.ArgumentExpressions);
            Assert.AreEqual("find", expression.MethodName);
        }
    }
}
