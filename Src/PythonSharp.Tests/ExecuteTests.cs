namespace PythonSharp.Tests
{
    using System;
    using System.Collections;
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
    public class ExecuteTests
    {
        private Machine machine;

        [TestInitialize]
        public void Setup()
        {
            this.machine = new Machine();
        }

        [TestMethod]
        public void ExecuteSimplePrint()
        {
            Assert.AreEqual("1\r\n", this.ExecuteAndPrint("print(1)"));
        }

        [TestMethod]
        public void ExecutePrintConstants()
        {
            Assert.AreEqual("None\r\n", this.ExecuteAndPrint("print(None)"));
            Assert.AreEqual("True\r\n", this.ExecuteAndPrint("print(True)"));
            Assert.AreEqual("False\r\n", this.ExecuteAndPrint("print(False)"));
        }

        [TestMethod]
        public void ExecutePrintWithSeparator()
        {
            Assert.AreEqual("1,2,3\r\n", this.ExecuteAndPrint("print(1,2,3,sep=',')"));
        }

        [TestMethod]
        public void ExecutePrintWithEmptyEnd()
        {
            Assert.AreEqual("1,2,3", this.ExecuteAndPrint("print(1,2,3,sep=',',end='')"));
        }

        [TestMethod]
        public void ExecutePrintWithNoneEnd()
        {
            Assert.AreEqual("1,2,3\r\n", this.ExecuteAndPrint("print(1,2,3,sep=',',end=None)"));
        }

        [TestMethod]
        public void ExecuteTwoSimplePrints()
        {
            Assert.AreEqual("1\r\n2\r\n", this.ExecuteAndPrint("print(1);print(2)"));
        }

        [TestMethod]
        [DeploymentItem("Examples/printvars.py")]
        public void ExecutePrintVars()
        {
            Assert.AreEqual("1\r\n2\r\n", this.ExecuteFileAndPrint("printvars.py"));
        }

        [TestMethod]
        [DeploymentItem("Examples/return.py")]
        public void ExecuteReturnFile()
        {
            Assert.AreEqual("1\r\n", this.ExecuteFileAndPrint("return.py"));
        }

        [TestMethod]
        [DeploymentItem("Examples/factorial.py")]
        public void ExecuteFactorialFile()
        {
            Assert.AreEqual("1\r\n2\r\n6\r\n24\r\n", this.ExecuteFileAndPrint("factorial.py"));
            var result = this.machine.Environment.GetValue("factorial");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedFunction));
            var function = (DefinedFunction)result;
            Assert.AreEqual("factorial function", function.GetValue("__doc__"));
        }

        [TestMethod]
        [DeploymentItem("Examples/defargs.py")]
        public void ExecuteDefArgsFile()
        {
            Assert.AreEqual("3\r\n4\r\n5\r\n", this.ExecuteFileAndPrint("defargs.py"));
        }

        [TestMethod]
        [DeploymentItem("Examples/namedargs.py")]
        public void ExecuteNamedArgsFile()
        {
            Assert.AreEqual("5\r\n7\r\n8\r\n", this.ExecuteFileAndPrint("namedargs.py"));
        }

        [TestMethod]
        [DeploymentItem("Examples/simpleclass.py")]
        public void ExecuteSimpleClassFile()
        {
            this.ExecuteFile("simpleclass.py");
            var result = this.machine.Environment.GetValue("Calculator");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IType));

            var type = (IType)result;

            Assert.IsNotNull(type.GetMethod("add"));
            Assert.IsNotNull(type.GetMethod("sub"));

            Assert.IsInstanceOfType(result, typeof(IContext));

            Assert.AreEqual(3.14159, ((IContext)result).GetValue("pi"));
        }

        [TestMethod]
        [DeploymentItem("Examples/complex.py")]
        public void ExecuteComplexFile()
        {
            this.ExecuteFile("complex.py");
            var result = this.machine.Environment.GetValue("x");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var dynobj = (DynamicObject)result;

            Assert.AreEqual(3.0, dynobj.GetValue("r"));
            Assert.AreEqual(4.5, dynobj.GetValue("i"));
        }

        [TestMethod]
        [DeploymentItem("Examples/atoms.py")]
        public void ExecuteAtomsFile()
        {
            Assert.IsTrue(this.ExecuteFileAndPrint("atoms.py").StartsWith("The mass of the second H atom is "));

            var atom = this.machine.Environment.GetValue("Atom");
            Assert.IsNotNull(atom);
            Assert.IsInstanceOfType(atom, typeof(DefinedClass));
            Assert.AreEqual("Atom class", ((DefinedClass)atom).GetValue("__doc__"));

            var atom1 = this.machine.Environment.GetValue("oAtom");
            Assert.IsNotNull(atom1);
            Assert.IsInstanceOfType(atom1, typeof(DynamicObject));
            var dynatom1 = (DynamicObject)atom1;
            Assert.AreEqual("O", dynatom1.GetValue("symbol"));
            Assert.AreEqual(15.9994, dynatom1.GetValue("mass"));
            Assert.IsInstanceOfType(dynatom1.GetValue("position"), typeof(IList));
        }

        [TestMethod]
        public void ExecuteSimpleFor()
        {
            Assert.AreEqual("1 2 3 ", this.ExecuteAndPrint("for k in [1,2,3]: print(k, end=' ')"));
        }

        [TestMethod]
        public void ExecuteTwoSimpleFor()
        {
            Assert.AreEqual("1 2 3 1 2 3 ", this.ExecuteAndPrint("for k in [1,2,3]: print(k, end=' ')\r\nfor k in [1,2,3]: print(k, end=' ')"));
        }

        [TestMethod]
        public void ExecuteSimpleOneLineFor()
        {
            Assert.AreEqual("1 2 3 ", this.ExecuteAndPrint("for k in [1,2,3]:\r\n  print(k, end=' ')"));
        }

        [TestMethod]
        public void ExecuteSimpleTwoLinesFor()
        {
            Assert.AreEqual("1 1 2 2 3 3 ", this.ExecuteAndPrint("for k in [1,2,3]:\r\n  print(k, end=' ')\r\n  print(k, end=' ')"));
        }

        [TestMethod]
        public void ExecuteSimpleTwoLinesAndBlankLineFor()
        {
            Assert.AreEqual("1 1 2 2 3 3 ", this.ExecuteAndPrint("for k in [1,2,3]:\r\n  print(k, end=' ')\r\n\r\n  print(k, end=' ')"));
        }

        [TestMethod]
        public void ExecuteSimpleTwoLinesForAndOneLine()
        {
            Assert.AreEqual("1 1 2 2 3 3 4 ", this.ExecuteAndPrint("for k in [1,2,3]:\r\n  print(k, end=' ')\r\n  print(k, end=' ')\r\nprint(4, end=' ')"));
        }

        [TestMethod]
        public void ExecuteSimpleSetIndex()
        {
            this.Execute("a = [1,2,3]");
            this.Execute("a[2] = 4");
            Assert.AreEqual(4, this.Evaluate("a[2]"));
            Assert.AreEqual(1, this.Evaluate("a[0]"));
            Assert.AreEqual(2, this.Evaluate("a[1]"));
        }

        [TestMethod]
        public void ExecuteSimpleSetDictionaryKey()
        {
            this.Execute("a = {}");
            this.Execute("a['foo'] = 'bar'");
            Assert.AreEqual("bar", this.Evaluate("a['foo']"));
        }

        private string ExecuteAndPrint(string text)
        {
            this.machine.Output = new StringWriter();
            this.Execute(text);
            return this.machine.Output.ToString();
        }

        private string ExecuteFileAndPrint(string filename)
        {
            this.machine.Output = new StringWriter();
            this.ExecuteFile(filename);
            return this.machine.Output.ToString();
        }

        private void Execute(string text)
        {
            Parser parser = new Parser(text);
            ICommand command = parser.CompileCommandList();
            command.Execute(this.machine.Environment);
        }

        private void ExecuteFile(string filename)
        {
            Parser parser = new Parser(new StreamReader(filename));
            ICommand command = parser.CompileCommandList();
            command.Execute(this.machine.Environment);
        }

        private object Evaluate(string text)
        {
            Parser parser = new Parser(text);
            IExpression expression = parser.CompileExpression();
            return expression.Evaluate(this.machine.Environment);
        }
    }
}
