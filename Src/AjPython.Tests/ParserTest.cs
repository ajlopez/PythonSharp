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
            Parser parser = new Parser(new Lexer("text"));

            Assert.IsNotNull(parser);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void RaiseIfParserIsNull()
        {
            Parser parser = new Parser((Lexer)null);
        }

        [TestMethod]
        public void CreateWithText()
        {
            Parser parser = new Parser("text");

            Assert.IsNotNull(parser);
        }

        [TestMethod]
        public void CreateWithReader()
        {
            Parser parser = new Parser(new StringReader("text"));

            Assert.IsNotNull(parser);
        }

        [TestMethod]
        public void CompileName()
        {
            Parser parser = new Parser("name");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(NameExpression));

            Assert.AreEqual("name", ((NameExpression)expression).Name);
        }

        [TestMethod]
        public void CompileInteger()
        {
            Parser parser = new Parser("123");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            Assert.AreEqual(123, ((ConstantExpression)expression).Value);

            Assert.IsNull(parser.CompileExpression());
        }

        [TestMethod]
        public void CompileReal()
        {
            Parser parser = new Parser("12.34");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            Assert.AreEqual(12.34, ((ConstantExpression)expression).Value);

            Assert.IsNull(parser.CompileExpression());
        }

        [TestMethod]
        public void CompileBoolean()
        {
            Parser parser = new Parser("true");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            Assert.AreEqual(true, ((ConstantExpression)expression).Value);

            Assert.IsNull(parser.CompileExpression());
        }

        [TestMethod]
        public void CompileString()
        {
            Parser parser = new Parser("\"foo\"");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            Assert.AreEqual("foo", ((ConstantExpression)expression).Value);
        }

        [TestMethod]
        public void CompileAndEvaluateAddExpression()
        {
            Parser parser = new Parser("1+2");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(3, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateSubtractExpression()
        {
            Parser parser = new Parser("1-2");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(-1, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateMultiplyExpression()
        {
            Parser parser = new Parser("2*3");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(6, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateDivideExpression()
        {
            Parser parser = new Parser("6/3");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(2, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluatePowerExpression()
        {
            Parser parser = new Parser("2**3");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(8, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateListExpression()
        {
            Parser parser = new Parser("[1,2,'foo']");

            IExpression expression = parser.CompileExpression();

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
            Parser parser = new Parser("[]");

            IExpression expression = parser.CompileExpression();

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
            Parser parser = new Parser("[a, b, c]");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);
            environment.SetValue("c", "foo");

            IExpression expression = parser.CompileExpression();

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
            Parser parser = new Parser("[a+1, b+2, c]");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);
            environment.SetValue("c", "foo");

            IExpression expression = parser.CompileExpression();

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
            Parser parser = new Parser("[1, 2, [a, b], 'foo']");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);

            IExpression expression = parser.CompileExpression();

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
            Parser parser = new Parser("{ 'firstname': 'foo', 'lastname': 'bar' }");

            IExpression expression = parser.CompileExpression();

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
            Parser parser = new Parser("{ }");

            IExpression expression = parser.CompileExpression();

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
            Parser parser = new Parser("6/3+5-2");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(5, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateExpressionUsingOperatorPrecedence()
        {
            Parser parser = new Parser("3+6/3");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(5, expression.Evaluate(new BindingEnvironment()));
        }

        [TestMethod]
        public void CompileAndEvaluateExpressionEnclosedInParenthesis()
        {
            Parser parser = new Parser("(6/3)");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(2, expression.Evaluate(new BindingEnvironment()));

            Assert.IsNull(parser.CompileExpression());
        }

        [TestMethod]
        public void CompileAndEvaluateComplexExpressionWithParenthesis()
        {
            Parser parser = new Parser("(6+3)/(1+2)");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryOperatorExpression));
            Assert.AreEqual(3, expression.Evaluate(new BindingEnvironment()));

            Assert.IsNull(parser.CompileExpression());
        }

        [TestMethod]
        public void CompileSimpleAssignmentCommand()
        {
            Parser parser = new Parser("foo = \"bar\"");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SimpleAssignmentCommand));
        }

        [TestMethod]
        [ExpectedException(typeof(NameExpectedException))]
        public void RaiseIsCommandDoesNotBeginWithName()
        {
            Parser parser = new Parser("123 = 12");

            ICommand command = parser.CompileCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedEndOfInputException))]
        public void RaiseIsCommandIsNotComplete()
        {
            Parser parser = new Parser("foo");

            ICommand command = parser.CompileCommand();
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void RaiseIsCommandIsUnknown()
        {
            Parser parser = new Parser("foo bar");

            ICommand command = parser.CompileCommand();
        }

        [TestMethod]
        public void CompilePrintCommand()
        {
            Parser parser = new Parser("print 'foo'");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(PrintCommand));
        }

        [TestMethod]
        public void CompileCompositeCommand()
        {
            Parser parser = new Parser("foo = \"bar\"\r\none = 1");

            ICommand command = parser.CompileCommandList();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));
        }

        [TestMethod]
        public void CompileCompositeCommandUsingSemicolon()
        {
            Parser parser = new Parser("foo = \"bar\";one = 1");

            ICommand command = parser.CompileCommandList();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));
        }

        [TestMethod]
        public void CompileCompositeCommandUsingSemicolonAndSpaces()
        {
            Parser parser = new Parser("foo = \"bar\";   one = 1");

            ICommand command = parser.CompileCommandList();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));
        }

        [TestMethod]
        public void CompileQualifiedNameExpression()
        {
            Parser parser = new Parser("module.foo");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(QualifiedNameExpression));

            QualifiedNameExpression qexpr = (QualifiedNameExpression)expression;

            Assert.AreEqual("module", qexpr.ModuleName);
            Assert.AreEqual("foo", qexpr.Name);
        }

        [TestMethod]
        public void CompileImportCommand()
        {
            Parser parser = new Parser("import module");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ImportCommand));

            ImportCommand impcmd = (ImportCommand)command;

            Assert.AreEqual("module", impcmd.ModuleName);
        }

        [TestMethod]
        public void CompileImportFromCommand()
        {
            Parser parser = new Parser("from module import a, b");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ImportFromCommand));

            ImportFromCommand impcmd = (ImportFromCommand)command;

            Assert.AreEqual("module", impcmd.ModuleName);

            Assert.IsNotNull(impcmd.Names);
            Assert.AreEqual(2, impcmd.Names.Count);
            Assert.AreEqual("a", impcmd.Names.First());
            Assert.AreEqual("b", impcmd.Names.Skip(1).First());
        }

        [TestMethod]
        [ExpectedException(typeof(SyntaxErrorException), "SyntaxError: invalid syntax")]
        public void RaiseIfInvalidSpacesAtStartOfLine()
        {
            Parser parser = new Parser("  a=1");

            parser.CompileCommand();
        }

        [TestMethod]
        public void CompileIfCommandWithSingleThenCommandSameLine()
        {
            Parser parser = new Parser("if a: print a");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsInstanceOfType(ifcmd.ThenCommand, typeof(PrintCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileIfCommandWithCompositeThenCommandSameLine()
        {
            Parser parser = new Parser("if a: print a; print b");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsInstanceOfType(ifcmd.ThenCommand, typeof(CompositeCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileIfCommandWithSingleThenCommand()
        {
            Parser parser = new Parser("if a:\r\n  print a");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsInstanceOfType(ifcmd.ThenCommand, typeof(PrintCommand));

            Assert.IsNull(parser.CompileCommand());
        }


        [TestMethod]
        public void CompileIfCommandWithCompositeThenCommand()
        {
            Parser parser = new Parser("if a:\r\n  print a\r\n  print b");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsInstanceOfType(ifcmd.ThenCommand, typeof(CompositeCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileAndExecuteIfCommandWithCompositeThenCommand()
        {
            Parser parser = new Parser("if 1:\r\n  one=1\r\n  two=2");
            Machine machine = new Machine();
            ICommand cmd = parser.CompileCommand();

            cmd.Execute(machine, machine.Environment);

            Assert.AreEqual(1, machine.Environment.GetValue("one"));
            Assert.AreEqual(2, machine.Environment.GetValue("two"));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileAndExecuteIfCommandWithCompositeThenCommandWithAnotherCommand()
        {
            Parser parser = new Parser("if 0:\r\n  one=1\r\n  two=2\r\nthree=3");
            Machine machine = new Machine();
            ICommand cmd = parser.CompileCommand();

            cmd.Execute(machine, machine.Environment);

            Assert.IsNull(machine.Environment.GetValue("one"));
            Assert.IsNull(machine.Environment.GetValue("two"));

            cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);

            cmd.Execute(machine, machine.Environment);

            Assert.AreEqual(3, machine.Environment.GetValue("three"));

            Assert.IsNull(parser.CompileCommand());
        }
    }
}

