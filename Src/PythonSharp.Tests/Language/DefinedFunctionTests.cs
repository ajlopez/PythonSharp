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
            IList<string> argumentNames = new string[] { "a", "b" };
            ICommand body = new SetCommand("c", new NameExpression("a"));
            DefinedFunction func = new DefinedFunction(argumentNames, body);

            Assert.AreEqual(argumentNames, func.ArgumentNames);
            Assert.AreEqual(body, func.Body);
        }

        [TestMethod]
        public void ExecuteFunctionWithPrint()
        {
            IList<string> argumentNames = new string[] { "a", "b" };
            CompositeCommand body = new CompositeCommand();
            body.AddCommand(new ExpressionCommand(new CallExpression(new NameExpression("print"), new IExpression[] { new NameExpression("a") })));
            body.AddCommand(new ExpressionCommand(new CallExpression(new NameExpression("print"), new IExpression[] { new NameExpression("b") })));

            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            DefinedFunction func = new DefinedFunction(argumentNames, body);

            func.Apply(machine.Environment, new object[] { 1, 2 });
            Assert.AreEqual("1\r\n2\r\n", writer.ToString());
        }
    }
}
