namespace AjPython.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AjPython;
    using AjPython.Commands;
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
        public void CompileReal()
        {
            Compiler compiler = new Compiler("12.34");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(RealExpression));

            Assert.AreEqual(12.34, ((RealExpression)expression).Value);

            Assert.IsNull(compiler.CompileExpression());
        }

        [TestMethod]
        public void CompileBoolean()
        {
            Compiler compiler = new Compiler("true");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BooleanExpression));

            Assert.AreEqual(true, ((BooleanExpression)expression).Value);

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
        public void CompileAndEvaluateSubtractExpression()
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
        public void CompileAndEvaluateDivideExpression()
        {
            Compiler compiler = new Compiler("6/3");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(2, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void CompileAndEvaluatePowerExpression()
        {
            Compiler compiler = new Compiler("2**3");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(8, expression.Evaluate(new Environment()));
        }

        [TestMethod]
        public void CompileAndEvaluateListExpression()
        {
            Compiler compiler = new Compiler("[1,2,'foo']");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));

            object result = expression.Evaluate(new Environment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList) result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual("foo", list[2]);
        }

        [TestMethod]
        public void CompileAndEvaluateNullListExpression()
        {
            Compiler compiler = new Compiler("[]");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));

            object result = expression.Evaluate(new Environment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void CompileAndEvaluateListWithVarsExpression()
        {
            Compiler compiler = new Compiler("[a, b, c]");
            Environment environment = new Environment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);
            environment.SetValue("c", "foo");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual("foo", list[2]);
        }

        [TestMethod]
        public void CompileAndEvaluateListWithExpressionsExpression()
        {
            Compiler compiler = new Compiler("[a+1, b+2, c]");
            Environment environment = new Environment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);
            environment.SetValue("c", "foo");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(2, list[0]);
            Assert.AreEqual(4, list[1]);
            Assert.AreEqual("foo", list[2]);
        }

        [TestMethod]
        public void CompileAndEvaluateComplexListExpression()
        {
            Compiler compiler = new Compiler("[1, 2, [a, b], 'foo']");
            Environment environment = new Environment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.IsNotNull(list[2]);
            Assert.IsInstanceOfType(list[2], typeof(IList));
            Assert.AreEqual("foo", list[3]);

            IList list2 = (IList)list[2];

            Assert.IsNotNull(list2);
            Assert.AreEqual(2, list2.Count);
            Assert.AreEqual(1, list2[0]);
            Assert.AreEqual(2, list2[1]);
        }

        [TestMethod]
        public void CompileAndEvaluateDictionaryExpression()
        {
            Compiler compiler = new Compiler("{ 'firstname': 'foo', 'lastname': 'bar' }");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(DictionaryExpression));

            object result = expression.Evaluate(new Environment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IDictionary));

            IDictionary dictionary = (IDictionary)result;

            Assert.AreEqual(2, dictionary.Keys.Count);
            Assert.AreEqual("foo", dictionary["firstname"]);
            Assert.AreEqual("bar", dictionary["lastname"]);
        }

        [TestMethod]
        public void CompileAndEvaluateNullDictionaryExpression()
        {
            Compiler compiler = new Compiler("{ }");

            Expression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(DictionaryExpression));

            object result = expression.Evaluate(new Environment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IDictionary));

            IDictionary dictionary = (IDictionary)result;

            Assert.AreEqual(0, dictionary.Keys.Count);
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
        public void CompileAndEvaluateExpressionUsingOperatorPrecedence()
        {
            Compiler compiler = new Compiler("3+6/3");

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

        [TestMethod]
        public void CompileSimpleAssignmentCommand()
        {
            Compiler compiler = new Compiler("foo = \"bar\"");

            Command command = compiler.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SimpleAssignmentCommand));
        }

        [TestMethod]
        [ExpectedException(typeof(NameExpectedException))]
        public void RaiseIsCommandDoesNotBeginWithName()
        {
            Compiler compiler = new Compiler("123 = 12");

            Command command = compiler.CompileCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedEndOfInputException))]
        public void RaiseIsCommandIsNotComplete()
        {
            Compiler compiler = new Compiler("foo");

            Command command = compiler.CompileCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void RaiseIsCommandIsUnknown()
        {
            Compiler compiler = new Compiler("foo bar");

            Command command = compiler.CompileCommand();
        }

        [TestMethod]
        public void CompilePrintCommand()
        {
            Compiler compiler = new Compiler("print 'foo'");

            Command command = compiler.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(PrintCommand));
        }
    }
}

