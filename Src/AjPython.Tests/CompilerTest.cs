namespace AjPython.Tests
{
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjPython;
    using AjPython.Compiler;
    using AjPython.Nodes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CompilerTest
    {
        [TestMethod]
        public void CreateWithParser()
        {
            Compiler compiler = new Compiler(new Parser("text"));

            Assert.IsNotNull(compiler);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfParserIsNull()
        {
            Compiler compiler = new Compiler((Parser)null);
        }

        [TestMethod]
        public void CreateWithText()
        {
            Compiler compiler = new Compiler("text");

            Assert.IsNotNull(compiler);
        }

        [TestMethod]
        public void CreateWithReader()
        {
            Compiler compiler = new Compiler(new StringReader("text"));

            Assert.IsNotNull(compiler);
        }

        [TestMethod]
        public void CompileName()
        {
            Compiler compiler = new Compiler("name");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(NameExpression));

            Assert.AreEqual("name", ((NameExpression)expression).Name);
        }

        [TestMethod]
        public void CompileInteger()
        {
            Compiler compiler = new Compiler("123");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(IntegerExpression));

            Assert.AreEqual(123, ((IntegerExpression)expression).Value);

            Assert.IsNull(compiler.CompileExpression());
        }

        [TestMethod]
        public void CompileString()
        {
            Compiler compiler = new Compiler("\"foo\"");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(StringExpression));

            Assert.AreEqual("foo", ((StringExpression)expression).Value);
        }

        [TestMethod]
        public void CompileAndEvaluateAddExpression()
        {
            Compiler compiler = new Compiler("1+2");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(3, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void CompileAndEvaluateSubstractExpression()
        {
            Compiler compiler = new Compiler("1-2");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(-1, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void CompileAndEvaluateMultiplyExpression()
        {
            Compiler compiler = new Compiler("2*3");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(6, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void CompileDivideExpression()
        {
            Compiler compiler = new Compiler("6/3");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(2, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void CompileAndEvaluateExpressionWithManyOperators()
        {
            Compiler compiler = new Compiler("6/3+5-2");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(5, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void CompileAndEvaluateExpressionEnclosedInParenthesis()
        {
            Compiler compiler = new Compiler("(6/3)");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(2, expression.Evaluate(new Environment()));

            Assert.IsNull(compiler.CompileExpression());
        }

        [TestMethod]
        public void CompileAndEvaluateComplexExpressionWithParenthesis()
        {
            Compiler compiler = new Compiler("(6+3)/(1+2)");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(3, expression.Evaluate(new Environment()));

            Assert.IsNull(compiler.CompileExpression());
        }
    }
}

