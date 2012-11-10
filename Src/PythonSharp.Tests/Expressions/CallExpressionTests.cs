namespace PythonSharp.Tests.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Functions;
    using PythonSharp.Language;
    using PythonSharp.Tests.Classes;
    using PythonSharp.Tests.Language;

    [TestClass]
    public class CallExpressionTests
    {
        [TestMethod]
        public void CallLen()
        {
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("len", new LenFunction());
            CallExpression expression = new CallExpression(new NameExpression("len"), new IExpression[] { new ConstantExpression("spam") });

            var result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
            Assert.IsNotNull(expression.TargetExpression);
            Assert.IsNotNull(expression.ArgumentExpressions);
        }

        [TestMethod]
        public void RaiseWhenCallLenWithNullArguments()
        {
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("len", new LenFunction());
            CallExpression expression = new CallExpression(new NameExpression("len"), null);

            try
            {
                expression.Evaluate(environment);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("len() takes exactly one argument (0 given)", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenCallWithNonKeywordArgumentAfterKeywordOne()
        {
            try
            {
                new CallExpression(new NameExpression("foo"), new IExpression[] { new NamedArgumentExpression("a", new ConstantExpression(1)), new ConstantExpression(2) });
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("non-keyword arg after keyword arg", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenKeywordArgumentRepeated()
        {
            try
            {
                new CallExpression(new NameExpression("foo"), new IExpression[] { new NamedArgumentExpression("a", new ConstantExpression(1)), new NamedArgumentExpression("a", new ConstantExpression(1)) });
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("keyword argument repeated", ex.Message);
            }
        }

        [TestMethod]
        public void CallObjectMethod()
        {
            DefinedClass klass = DynamicObjectTests.CreateClassWithMethods("Spam");
            DynamicObject dynobj = (DynamicObject) klass.Apply(null, null, null);
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("foo", dynobj);
            CallExpression expression = new CallExpression(new AttributeExpression(new NameExpression("foo"), "getSelf"), null);

            var result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.AreEqual(dynobj, result);
        }

        [TestMethod]
        public void CallNativeObjectMethodWithoutArguments()
        {
            Person person = new Person() { FirstName = "Adam", LastName = "Doe" };
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("adam", person);
            CallExpression expression = new CallExpression(new AttributeExpression(new NameExpression("adam"), "GetName"), null);

            var result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.AreEqual(person.GetName(), result);
        }

        [TestMethod]
        public void CallNativeObjectMethodWithArguments()
        {
            Calculator calculator = new Calculator();
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("calculator", calculator);
            CallExpression expression = new CallExpression(new AttributeExpression(new NameExpression("calculator"), "Add"), new IExpression[] { new ConstantExpression(1), new ConstantExpression(2) });

            var result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.AreEqual(calculator.Add(1, 2), result);
        }

        [TestMethod]
        public void CallNativeType()
        {
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("FileInfo", typeof(System.IO.FileInfo));
            CallExpression expression = new CallExpression(new NameExpression("FileInfo"), new IExpression[] { new ConstantExpression("unknown.txt") });

            var result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(System.IO.FileInfo));

            var fileinfo = (System.IO.FileInfo)result;

            Assert.AreEqual("unknown.txt", fileinfo.Name);
        }
    }
}
