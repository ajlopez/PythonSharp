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
    public class ParameterExpressionTests
    {
        [TestMethod]
        public void CreateAndEvaluateParameterExpressionWithName()
        {
            var expression = new ParameterExpression("a", null, false);

            var result = expression.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Parameter));

            var param = (Parameter)result;

            Assert.AreEqual("a", expression.Name);
            Assert.IsNull(expression.DefaultExpression);
            Assert.IsFalse(expression.IsList);

            Assert.AreEqual("a", param.Name);
            Assert.IsNull(param.DefaultValue);
            Assert.IsFalse(param.IsList);
        }

        [TestMethod]
        public void CreateAndEvaluateParameterExpressionWithNameAndList()
        {
            var expression = new ParameterExpression("a", null, true);

            var result = expression.Evaluate(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Parameter));

            var param = (Parameter)result;

            Assert.AreEqual("a", expression.Name);
            Assert.IsNull(expression.DefaultExpression);
            Assert.IsTrue(expression.IsList);

            Assert.AreEqual("a", param.Name);
            Assert.IsNull(param.DefaultValue);
            Assert.IsTrue(param.IsList);
        }

        [TestMethod]
        public void CreateAndEvaluateParameterExpressionWithNameAndDefaultExpression()
        {
            var environment = new BindingEnvironment();
            environment.SetValue("b", 1);
            var defaultExpression = new NameExpression("b");
            var expression = new ParameterExpression("a", defaultExpression, false);

            var result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Parameter));

            var param = (Parameter)result;

            Assert.AreEqual("a", expression.Name);
            Assert.AreEqual(defaultExpression, expression.DefaultExpression);
            Assert.IsFalse(expression.IsList);

            Assert.AreEqual("a", param.Name);
            Assert.AreEqual(1, param.DefaultValue);
            Assert.IsFalse(param.IsList);
        }
    }
}
