namespace PythonSharp.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    [TestClass]
    public class DefinedFunctionTests
    {
        [TestMethod]
        public void CreateSimpleDefinedFunction()
        {
            IList<Parameter> parameters = new Parameter[] { new Parameter("a", null, false), new Parameter("b", null, false) };
            ICommand body = new SetCommand("c", new NameExpression("a"));
            DefinedFunction func = new DefinedFunction(parameters, body);

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

            DefinedFunction func = new DefinedFunction(parameters, body);

            func.Apply(machine.Environment, new object[] { 1, 2 });
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

            DefinedFunction func = new DefinedFunction(parameters, body);

            var result = func.Apply(machine.Environment, new object[] { 1, 2 });

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
        }
    }
}
