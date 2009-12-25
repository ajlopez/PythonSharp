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
    using AjPython.Expressions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void CreateWithParser()
        {
            Parser compiler = new Parser(new Lexer("text"));

            Assert.IsNotNull(compiler);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfParserIsNull()
        {
            Parser compiler = new Parser((Lexer)null);
        }

        [TestMethod]
        public void CreateWithText()
        {
            Parser compiler = new Parser("text");

            Assert.IsNotNull(compiler);
        }

        [TestMethod]
        public void CreateWithReader()
        {
            Parser compiler = new Parser(new StringReader("text"));

            Assert.IsNotNull(compiler);
        }

        [TestMethod]
        public void CompileName()
        {
            Parser compiler = new Parser("name");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(NameExpression));

            Assert.AreEqual("name", ((NameExpression)expression).Name);
        }

        [TestMethod]
        public void CompileInteger()
        {
            Parser compiler = new Parser("123");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(IntegerExpression));

            Assert.AreEqual(123, ((IntegerExpression)expression).Value);

            Assert.IsNull(compiler.CompileExpression());
        }

        [TestMethod]
        public void CompileReal()
        {
            Parser compiler = new Parser("12.34");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(RealExpression));

            Assert.AreEqual(12.34, ((RealExpression)expression).Value);

            Assert.IsNull(compiler.CompileExpression());
        }

        [TestMethod]
        public void CompileBoolean()
        {
            Parser compiler = new Parser("true");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BooleanExpression));

            Assert.AreEqual(true, ((BooleanExpression)expression).Value);

            Assert.IsNull(compiler.CompileExpression());
        }

        [TestMethod]
        public void CompileString()
        {
            Parser compiler = new Parser("\"foo\"");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(StringExpression));

            Assert.AreEqual("foo", ((StringExpression)expression).Value);
        }

        [TestMethod]
        public void CompileAndEvaluateAddExpression()
        {
            Parser compiler = new Parser("1+2");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(3, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateSubtractExpression()
        {
            Parser compiler = new Parser("1-2");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(-1, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateMultiplyExpression()
        {
            Parser compiler = new Parser("2*3");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(6, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateDivideExpression()
        {
            Parser compiler = new Parser("6/3");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(2, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluatePowerExpression()
        {
            Parser compiler = new Parser("2**3");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(8, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateListExpression()
        {
            Parser compiler = new Parser("[1,2,'foo']");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));

            object result = expression.Evaluate(new BindingEnvironment());

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
            Parser compiler = new Parser("[]");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));

            object result = expression.Evaluate(new BindingEnvironment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void CompileAndEvaluateListWithVarsExpression()
        {
            Parser compiler = new Parser("[a, b, c]");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);
            environment.SetValue("c", "foo");

            IExpression expression = compiler.CompileExpression();

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
            Parser compiler = new Parser("[a+1, b+2, c]");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);
            environment.SetValue("c", "foo");

            IExpression expression = compiler.CompileExpression();

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
            Parser compiler = new Parser("[1, 2, [a, b], 'foo']");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);

            IExpression expression = compiler.CompileExpression();

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
            Parser compiler = new Parser("{ 'firstname': 'foo', 'lastname': 'bar' }");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(DictionaryExpression));

            object result = expression.Evaluate(new BindingEnvironment());

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
            Parser compiler = new Parser("{ }");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(DictionaryExpression));

            object result = expression.Evaluate(new BindingEnvironment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IDictionary));

            IDictionary dictionary = (IDictionary)result;

            Assert.AreEqual(0, dictionary.Keys.Count);
        }

        [TestMethod]
        public void CompileAndEvaluateExpressionWithManyOperators()
        {
            Parser compiler = new Parser("6/3+5-2");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(5, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateExpressionUsingOperatorPrecedence()
        {
            Parser compiler = new Parser("3+6/3");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(5, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateExpressionEnclosedInParenthesis()
        {
            Parser compiler = new Parser("(6/3)");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(2, expression.Evaluate(new BindingEnvironment()));

            Assert.IsNull(compiler.CompileExpression());
        }

        [TestMethod]
        public void CompileAndEvaluateComplexExpressionWithParenthesis()
        {
            Parser compiler = new Parser("(6+3)/(1+2)");

            IExpression expression = compiler.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(3, expression.Evaluate(new BindingEnvironment()));

            Assert.IsNull(compiler.CompileExpression());
        }

        [TestMethod]
        public void CompileSimpleAssignmentCommand()
        {
            Parser compiler = new Parser("foo = \"bar\"");

            ICommand command = compiler.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SimpleAssignmentCommand));
        }

        [TestMethod]
        [ExpectedException(typeof(NameExpectedException))]
        public void RaiseIsCommandDoesNotBeginWithName()
        {
            Parser compiler = new Parser("123 = 12");

            ICommand command = compiler.CompileCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedEndOfInputException))]
        public void RaiseIsCommandIsNotComplete()
        {
            Parser compiler = new Parser("foo");

            ICommand command = compiler.CompileCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void RaiseIsCommandIsUnknown()
        {
            Parser compiler = new Parser("foo bar");

            ICommand command = compiler.CompileCommand();
        }

        [TestMethod]
        public void CompilePrintCommand()
        {
            Parser compiler = new Parser("print 'foo'");

            ICommand command = compiler.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(PrintCommand));
        }
    }
}

