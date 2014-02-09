namespace PythonSharp.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Language;
    using PythonSharp.Utilities;

    [TestClass]
    public class DefinedFunctionTests
    {
        [TestMethod]
        public void CreateSimpleDefinedFunction()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false), new Parameter("b", null, false) };
            ICommand body = new SetCommand("c", new NameExpression("a"));
            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            Assert.AreEqual(parameters, func.Parameters);
            Assert.AreEqual(body, func.Body);
        }

        [TestMethod]
        public void ExecuteFunctionWithPrint()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false), new Parameter("b", null, false) };
            CompositeCommand body = new CompositeCommand();
            body.AddCommand(new ExpressionCommand(new CallExpression(new NameExpression("print"), new IExpression[] { new NameExpression("a") })));
            body.AddCommand(new ExpressionCommand(new CallExpression(new NameExpression("print"), new IExpression[] { new NameExpression("b") })));

            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            DefinedFunction func = new DefinedFunction("foo", parameters, body, machine.Environment);

            func.Apply(machine.Environment, new object[] { 1, 2 }, null);
            Assert.AreEqual("1\r\n2\r\n", writer.ToString());
        }

        [TestMethod]
        public void ExecuteFunctionWithReturn()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false), new Parameter("b", null, false) };
            CompositeCommand body = new CompositeCommand();
            body.AddCommand(new ReturnCommand(new BinaryOperatorExpression(new NameExpression("a"), new NameExpression("b"), BinaryOperator.Add)));

            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            var result = func.Apply(machine.Environment, new object[] { 1, 2 }, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void RaiseWhenFewParametersProvided()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false), new Parameter("b", null, false) };
            CompositeCommand body = new CompositeCommand();

            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            try
            {
                func.Apply(machine.Environment, new object[] { 1 }, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("foo() takes exactly 2 positional arguments (1 given)", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenFewerThanExpectedParametersProvided()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false), new Parameter("b", new ConstantExpression(1), false) };
            CompositeCommand body = new CompositeCommand();

            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            try
            {
                func.Apply(machine.Environment, new object[] { }, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("foo() takes at least 1 positional argument (0 given)", ex.Message);
            }
        }

        [TestMethod]
        public void RaiseWhenOneParameterExpectedAndNoneIsProvided()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false) };
            CompositeCommand body = new CompositeCommand();

            Machine machine = new Machine();

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            try
            {
                func.Apply(machine.Environment, null, null);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("foo() takes exactly 1 positional argument (0 given)", ex.Message);
            }
        }

        [TestMethod]
        public void EvaluateUsingDefaultValuesForArguments()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", 1, false), new Parameter("b", 2, false) };
            ICommand body = new ReturnCommand(new BinaryOperatorExpression(new NameExpression("a"), new NameExpression("b"), BinaryOperator.Add));

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            Assert.AreEqual(3, func.Apply(new BindingEnvironment(), null, null));
        }

        [TestMethod]
        public void EvaluateUsingListArgument()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false), new Parameter("b", null, true) };
            ICommand body = new ReturnCommand(new NameExpression("b"));

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            var result = func.Apply(new BindingEnvironment(), new object[] { 1, 2, 3 }, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<object>));

            var list = (IList<object>)result;

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(2, list[0]);
            Assert.AreEqual(3, list[1]);
        }

        [TestMethod]
        public void EvaluateUsingEmptyListArgument()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false), new Parameter("b", null, true) };
            ICommand body = new ReturnCommand(new NameExpression("b"));

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            var result = func.Apply(new BindingEnvironment(), new object[] { 1 }, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<object>));

            var list = (IList<object>)result;

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void EvaluateUsingEmptyListArgumentWithDefaultValue()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false), new Parameter("b", new object[] { 1, 2 }, true) };
            ICommand body = new ReturnCommand(new NameExpression("b"));

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            var result = func.Apply(new BindingEnvironment(), new object[] { 1 }, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<object>));

            var list = (IList<object>)result;

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
        }

        [TestMethod]
        public void EvaluateUsingNamedArgument()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", 1, false), new Parameter("b", 2, false) };
            ICommand body = new ReturnCommand(new BinaryOperatorExpression(new NameExpression("a"), new NameExpression("b"), BinaryOperator.Add));

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            var result = func.Apply(new BindingEnvironment(), null, new Dictionary<string, object> { { "a", 2 } });

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void EvaluateUsingTwoNamedArguments()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", 1, false), new Parameter("b", 2, false) };
            ICommand body = new ReturnCommand(new BinaryOperatorExpression(new NameExpression("a"), new NameExpression("b"), BinaryOperator.Add));

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            var result = func.Apply(new BindingEnvironment(), null, new Dictionary<string, object> { { "a", 2 }, { "b", 3 } });

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void RaiseWhenMultipleValuesForKeywordArgument()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", 1, false), new Parameter("b", 2, false) };
            ICommand body = new ReturnCommand(new BinaryOperatorExpression(new NameExpression("a"), new NameExpression("b"), BinaryOperator.Add));

            DefinedFunction func = new DefinedFunction("foo", parameters, body, null);

            try
            {
                func.Apply(new BindingEnvironment(), new object[] { 1 }, new Dictionary<string, object> { { "a", 2 } });
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("foo() got multiple values for keyword argument 'a'", ex.Message);
            }
        }

        [TestMethod]
        public void ValuesInFunction()
        {
            ICommand body = new PassCommand();

            DefinedFunction func = new DefinedFunction("foo", null, body, null);

            func.SetValue("__doc__", "foo function");

            Assert.IsFalse(func.HasValue("bar"));
            Assert.IsTrue(func.HasValue("__doc__"));
            Assert.AreEqual("foo function", func.GetValue("__doc__"));
        }

        [TestMethod]
        public void GetAndUseDelegate()
        {
            ICommand body = new PassCommand();

            DefinedFunction func = new DefinedFunction("foo", null, body, null);
            var type = typeof(ThreadStart);

            // Activator.CreateInstance(typeof(ThreadStart), func.DoFunction);
        }
    }
}
