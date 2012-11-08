namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Compiler;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    [TestClass]
    public class EvaluateTests
    {
        private Machine machine;

        [TestInitialize]
        public void Setup()
        {
            this.machine = new Machine();
        }

        [TestMethod]
        public void EvaluateSimpleInteger()
        {
            Assert.AreEqual(1, this.Evaluate("1"));
        }

        [TestMethod]
        public void EvaluateSimpleSum()
        {
            Assert.AreEqual(3, this.Evaluate("1+2"));
        }

        [TestMethod]
        public void EvaluateSimpleArithmeticExpression()
        {
            Assert.AreEqual(14, this.Evaluate("2+3*4"));
        }

        [TestMethod]
        public void EvaluateSimpleArithmeticExpressionWithParenthesis()
        {
            Assert.AreEqual(20, this.Evaluate("(2+3)*4"));
        }

        [TestMethod]
        public void EvaluateMethodCall()
        {
            DefinedClass klass = new DefinedClass("Spam");
            DefinedFunction function = new DefinedFunction("get", new Parameter[] { new Parameter("self", null, false), new Parameter("a", null, false) }, new ReturnCommand(new NameExpression("a")));
            klass.SetMethod(function.Name, function);
            DynamicObject foo = (DynamicObject)klass.Apply(this.machine.Environment, null, null);
            this.machine.Environment.SetValue("foo", foo);
            Assert.AreEqual(2, this.Evaluate("foo.get(2)"));
        }

        private object Evaluate(string text)
        {
            Parser parser = new Parser(text);
            IExpression expression = parser.CompileExpression();
            return expression.Evaluate(this.machine.Environment);
        }
    }
}
