namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Language;
    using PythonSharp.Tests.Classes;

    [TestClass]
    public class AttributeExpressionTests
    {
        [TestMethod]
        public void EvaluateAttributeExpression()
        {
            AttributeExpression expression = new AttributeExpression(new NameExpression("module"), "foo");
            BindingEnvironment environment = new BindingEnvironment();
            BindingEnvironment modenv = new BindingEnvironment();

            modenv.SetValue("foo", "bar");
            environment.SetValue("module", modenv);

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("bar", result);
            Assert.IsNotNull(expression.Expression);
            Assert.AreEqual("foo", expression.Name);
        }

        [TestMethod]
        public void EvaluateAttributeExpressionOnNativeObjectProperty()
        {
            AttributeExpression expression = new AttributeExpression(new NameExpression("adam"), "FirstName");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("adam", new Person() { FirstName = "Adam" });

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("Adam", result);
        }

        [TestMethod]
        public void EvaluateAttributeExpressionOnClassProperty()
        {
            AttributeExpression expression = new AttributeExpression(new NameExpression("Calculator"), "Value");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("Calculator", typeof(Calculator));

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void RaiseWhenEvaluateAttributeExpression()
        {
            AttributeExpression expression = new AttributeExpression(new NameExpression("module"), "spam");
            BindingEnvironment environment = new BindingEnvironment();
            BindingEnvironment modenv = new BindingEnvironment();

            environment.SetValue("module", modenv);

            try
            {
                expression.Evaluate(environment);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(AttributeError));
                Assert.AreEqual("'module' object has no attribute 'spam'", ex.Message);
            }
        }

        [TestMethod]
        public void GetNativeMethod()
        {
            AttributeExpression expression = new AttributeExpression(new ConstantExpression(1), "GetType");
            BindingEnvironment environment = new BindingEnvironment();

            var result = expression.Evaluate(environment);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MethodInfo));
        }
    }
}

