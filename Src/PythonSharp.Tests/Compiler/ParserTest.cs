namespace PythonSharp.Tests.Compiler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;
    using PythonSharp.Commands;
    using PythonSharp.Compiler;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

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
            Parser parser = new Parser("\"spam\"");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            Assert.AreEqual("spam", ((ConstantExpression)expression).Value);
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
        public void CompileExpressionCommand()
        {
            Parser parser = new Parser("1+2");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));
            command.Execute(null);

            ExpressionCommand exprcommand = (ExpressionCommand)command;
            Assert.IsNotNull(exprcommand.Expression);
            Assert.AreEqual(3, exprcommand.Expression.Evaluate(null));
        }

        [TestMethod]
        public void CompileExpressionCommandWithName()
        {
            Parser parser = new Parser("a+2");
            BindingEnvironment environment = new BindingEnvironment();
            environment.SetValue("a", 1);

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));
            command.Execute(environment);

            ExpressionCommand exprcommand = (ExpressionCommand)command;
            Assert.IsNotNull(exprcommand.Expression);
            Assert.AreEqual(3, exprcommand.Expression.Evaluate(environment));
        }

        [TestMethod]
        public void CompileExpressionCommandWithList()
        {
            Parser parser = new Parser("1, 2");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));
            command.Execute(null);

            ExpressionCommand exprcommand = (ExpressionCommand)command;
            Assert.IsNotNull(exprcommand.Expression);
            Assert.IsInstanceOfType(exprcommand.Expression, typeof(ListExpression));
            Assert.IsTrue(((ListExpression)exprcommand.Expression).IsReadOnly);
        }

        [TestMethod]
        public void CompileSimpleExpressionInParenthesis()
        {
            Parser parser = new Parser("(1)");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void CompileListExpressionInParenthesis()
        {
            Parser parser = new Parser("(1, 2)");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));

            var lexpr = (ListExpression)expression;

            Assert.IsTrue(lexpr.IsReadOnly);
            Assert.AreEqual(2, lexpr.Expressions.Count);
        }

        [TestMethod]
        public void CompileEmptyTupleExpression()
        {
            Parser parser = new Parser("()");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));

            var lexpr = (ListExpression)expression;

            Assert.IsTrue(lexpr.IsReadOnly);
            Assert.AreEqual(0, lexpr.Expressions.Count);
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
        public void CompileAndEvaluateArrayAsListExpression()
        {
            Parser parser = new Parser("[1,2,'spam']");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));
            Assert.IsFalse(((ListExpression)expression).IsReadOnly);

            object result = expression.Evaluate(new BindingEnvironment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual("spam", list[2]);
        }

        [TestMethod]
        public void CompileAndEvaluateNullListExpression()
        {
            Parser parser = new Parser("[]");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));
            Assert.IsFalse(((ListExpression)expression).IsReadOnly);

            object result = expression.Evaluate(new BindingEnvironment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectedTokenException))]
        public void RaiseIfCompileListWithMissingComma()
        {
            Parser parser = new Parser("[1 2]");
            parser.CompileExpression();
        }

        [TestMethod]
        public void CompileAndEvaluateListWithVarsExpression()
        {
            Parser parser = new Parser("[a, b, c]");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);
            environment.SetValue("c", "spam");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));
            Assert.IsFalse(((ListExpression)expression).IsReadOnly);

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual("spam", list[2]);
        }

        [TestMethod]
        public void CompileAndEvaluateListWithExpressionsExpression()
        {
            Parser parser = new Parser("[a+1, b+2, c]");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);
            environment.SetValue("c", "spam");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));
            Assert.IsFalse(((ListExpression)expression).IsReadOnly);

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(2, list[0]);
            Assert.AreEqual(4, list[1]);
            Assert.AreEqual("spam", list[2]);
        }

        [TestMethod]
        public void CompileAndEvaluateComplexListExpression()
        {
            Parser parser = new Parser("[1, 2, [a, b], 'spam']");
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("a", 1);
            environment.SetValue("b", 2);

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ListExpression));
            Assert.IsFalse(((ListExpression)expression).IsReadOnly);

            object result = expression.Evaluate(environment);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList list = (IList)result;

            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.IsNotNull(list[2]);
            Assert.IsInstanceOfType(list[2], typeof(IList));
            Assert.AreEqual("spam", list[3]);

            IList list2 = (IList)list[2];

            Assert.IsNotNull(list2);
            Assert.AreEqual(2, list2.Count);
            Assert.AreEqual(1, list2[0]);
            Assert.AreEqual(2, list2[1]);
        }

        [TestMethod]
        public void CompileAndEvaluateDictionaryExpression()
        {
            Parser parser = new Parser("{ 'firstname': 'spam', 'lastname': 'bar' }");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(DictionaryExpression));

            object result = expression.Evaluate(new BindingEnvironment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IDictionary));

            IDictionary dictionary = (IDictionary)result;

            Assert.AreEqual(2, dictionary.Keys.Count);
            Assert.AreEqual("spam", dictionary["firstname"]);
            Assert.AreEqual("bar", dictionary["lastname"]);
        }

        [TestMethod]
        public void CompileAndEvaluateNullDictionaryExpression()
        {
            Parser parser = new Parser("{ }");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(DictionaryExpression));

            var dictexpr = (DictionaryExpression)expression;

            Assert.AreEqual(0, dictexpr.KeyExpressions.Count);
            Assert.AreEqual(0, dictexpr.ValueExpressions.Count);

            object result = expression.Evaluate(new BindingEnvironment());

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IDictionary));

            IDictionary dictionary = (IDictionary)result;

            Assert.AreEqual(0, dictionary.Keys.Count);
            Assert.AreEqual(0, dictionary.Values.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectedTokenException))]
        public void RaiseIfDictionaryExpressionWithMissingComma()
        {
            Parser parser = new Parser("{ 'firstname': 'spam' 'lastname': 'bar' }");
            parser.CompileExpression();
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectedTokenException))]
        public void RaiseIfDictionaryExpressionWithMissingPeriod()
        {
            Parser parser = new Parser("{ 'firstname' 'spam', 'lastname': 'bar' }");
            parser.CompileExpression();
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
            Parser parser = new Parser("spam = \"bar\"");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SetCommand));
        }

        [TestMethod]
        public void CompileNameAsExpression()
        {
            Parser parser = new Parser("spam");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));

            ExpressionCommand exprcommand = (ExpressionCommand)command;
            Assert.IsNotNull(exprcommand.Expression);
            Assert.IsInstanceOfType(exprcommand.Expression, typeof(NameExpression));
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void RaiseIsCommandIsUnknown()
        {
            Parser parser = new Parser("spam bar");

            ICommand command = parser.CompileCommand();
        }

        [TestMethod]
        public void CompilePrintFunction()
        {
            Parser parser = new Parser("print('spam')");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));

            var exprcmd = (ExpressionCommand)command;

            Assert.IsNotNull(exprcmd.Expression);
            Assert.IsInstanceOfType(exprcmd.Expression, typeof(CallExpression));

            var callexpr = (CallExpression)exprcmd.Expression;

            Assert.IsNotNull(callexpr.TargetExpression);
            Assert.IsInstanceOfType(callexpr.TargetExpression, typeof(NameExpression));
            Assert.IsNotNull(callexpr.ArgumentExpressions);
            Assert.AreEqual(1, callexpr.ArgumentExpressions.Count);
        }

        [TestMethod]
        public void CompilePrintFunctionWithTwoArguments()
        {
            Parser parser = new Parser("print('spam', 'bar')");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));

            var exprcmd = (ExpressionCommand)command;
            
            Assert.IsNotNull(exprcmd.Expression);
            Assert.IsInstanceOfType(exprcmd.Expression, typeof(CallExpression));

            var callexpr = (CallExpression)exprcmd.Expression;

            Assert.IsNotNull(callexpr.TargetExpression);
            Assert.IsInstanceOfType(callexpr.TargetExpression, typeof(NameExpression));
            Assert.IsNotNull(callexpr.ArgumentExpressions);
            Assert.AreEqual(2, callexpr.ArgumentExpressions.Count);
        }

        [TestMethod]
        public void CompileEmptyPrintFunction()
        {
            Parser parser = new Parser("print()");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));

            var exprcmd = (ExpressionCommand)command;

            Assert.IsNotNull(exprcmd.Expression);
            Assert.IsInstanceOfType(exprcmd.Expression, typeof(CallExpression));

            var callexpr = (CallExpression)exprcmd.Expression;

            Assert.IsNotNull(callexpr.TargetExpression);
            Assert.IsInstanceOfType(callexpr.TargetExpression, typeof(NameExpression));
            Assert.IsNull(callexpr.ArgumentExpressions);
        }

        [TestMethod]
        public void CompileCompositeCommand()
        {
            Parser parser = new Parser("spam = \"bar\"\r\none = 1");

            ICommand command = parser.CompileCommandList();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));
        }

        [TestMethod]
        public void CompileEmptyCommandList()
        {
            Parser parser = new Parser(string.Empty);

            ICommand command = parser.CompileCommandList();

            Assert.IsNull(command);
        }

        [TestMethod]
        public void CompileCompositeCommandUsingSemicolon()
        {
            Parser parser = new Parser("spam = \"bar\";one = 1");

            ICommand command = parser.CompileCommandList();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));
        }

        [TestMethod]
        public void CompileCompositeCommandUsingSemicolonAndSpaces()
        {
            Parser parser = new Parser("spam = \"bar\";   one = 1");

            ICommand command = parser.CompileCommandList();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(CompositeCommand));
        }

        [TestMethod]
        public void CompileAttributeExpression()
        {
            Parser parser = new Parser("module.spam");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(AttributeExpression));

            AttributeExpression attrexpr = (AttributeExpression)expression;

            Assert.IsNotNull(attrexpr.Expression);
            Assert.IsInstanceOfType(attrexpr.Expression, typeof(NameExpression));
            Assert.AreEqual("spam", attrexpr.Name);
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
        public void CompileImportCommandWithDottedName()
        {
            Parser parser = new Parser("import PythonSharp.Language");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ImportCommand));

            ImportCommand impcmd = (ImportCommand)command;

            Assert.AreEqual("PythonSharp.Language", impcmd.ModuleName);
        }

        [TestMethod]
        [ExpectedException(typeof(NameExpectedException))]
        public void RaiseIfCompileImportCommandWithoutModuleName()
        {
            Parser parser = new Parser("import");
            parser.CompileCommand();
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
        public void CompileImportAllFromCommand()
        {
            Parser parser = new Parser("from module import *");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ImportFromCommand));

            ImportFromCommand impcmd = (ImportFromCommand)command;

            Assert.AreEqual("module", impcmd.ModuleName);

            Assert.IsNull(impcmd.Names);
        }

        [TestMethod]
        public void CompileImportCompositeName()
        {
            Parser parser = new Parser("from System.IO import *");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ImportFromCommand));

            ImportFromCommand impcmd = (ImportFromCommand)command;

            Assert.AreEqual("System.IO", impcmd.ModuleName);

            Assert.IsNull(impcmd.Names);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpectedTokenException))]
        public void RaiseIfCompileImportFromCommandWithNoImportWord()
        {
            Parser parser = new Parser("from module imports a, b");
            parser.CompileCommand();
        }

        [TestMethod]
        public void RaiseIfInvalidSpacesAtStartOfLine()
        {
            Parser parser = new Parser("  a=1");

            try
            {
                parser.CompileCommand();
                Assert.Fail("Expected Exception");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("unexpected indent", ex.Message);
            }
        }

        [TestMethod]
        public void CompileIfCommandWithSingleThenCommandSameLine()
        {
            Parser parser = new Parser("if a: print(a)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsInstanceOfType(ifcmd.ThenCommand, typeof(ExpressionCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileIfCommandWithCompositeThenCommandSameLine()
        {
            Parser parser = new Parser("if a: print(a); print(b)");

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
            Parser parser = new Parser("if a:\r\n  print(a)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsInstanceOfType(ifcmd.ThenCommand, typeof(ExpressionCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileIfCommandWithCompositeThenCommand()
        {
            Parser parser = new Parser("if a:\r\n  print(a)\r\n  print(b)");

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

            cmd.Execute(machine.Environment);

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

            cmd.Execute(machine.Environment);

            Assert.IsNull(machine.Environment.GetValue("one"));
            Assert.IsNull(machine.Environment.GetValue("two"));

            cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);

            cmd.Execute(machine.Environment);

            Assert.AreEqual(3, machine.Environment.GetValue("three"));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedEndOfInputException))]
        public void RaiseIfCompileIfHasNoCommand()
        {
            Parser parser = new Parser("if 0:");
            parser.CompileCommand();
        }

        [TestMethod]
        public void CompileExpressionList()
        {
            Parser parser = new Parser("a, b");

            IList<IExpression> expressions = parser.CompileExpressionList();

            Assert.IsNotNull(expressions);
            Assert.AreEqual(2, expressions.Count);
            Assert.IsInstanceOfType(expressions[0], typeof(NameExpression));
            Assert.IsInstanceOfType(expressions[1], typeof(NameExpression));
        }

        [TestMethod]
        public void CompileCompareExpression()
        {
            Parser parser = new Parser("a < b");

            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(CompareExpression));

            Assert.IsNull(parser.CompileExpression());
        }

        [TestMethod]
        public void EvaluateNumericComparison()
        {
            Assert.IsTrue((bool)CompileAndEvaluateExpression("1 < 2"));
            Assert.IsTrue((bool)CompileAndEvaluateExpression("1 <= 2"));
            Assert.IsTrue((bool)CompileAndEvaluateExpression("3 > 2"));
            Assert.IsTrue((bool)CompileAndEvaluateExpression("3 >= 2"));
            Assert.IsTrue((bool)CompileAndEvaluateExpression("3 <> 2"));
            Assert.IsTrue((bool)CompileAndEvaluateExpression("2 == 2"));
            Assert.IsFalse((bool)CompileAndEvaluateExpression("3 == 2"));
            Assert.IsTrue((bool)CompileAndEvaluateExpression("3 != 2"));
        }

        [TestMethod]
        public void CompileNoneAsNullConstantExpression()
        {
            Parser parser = new Parser("None");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression cexpr = (ConstantExpression)expression;

            Assert.IsNull(cexpr.Value);
        }

        [TestMethod]
        public void CompileTrueAsBooleanConstantExpression()
        {
            Parser parser = new Parser("True");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression cexpr = (ConstantExpression)expression;

            Assert.AreEqual(true, cexpr.Value);
        }

        [TestMethod]
        public void CompileFalseAsBooleanConstantExpression()
        {
            Parser parser = new Parser("False");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression cexpr = (ConstantExpression)expression;

            Assert.AreEqual(false, cexpr.Value);
        }

        [TestMethod]
        public void CompileIndexedExpression()
        {
            Parser parser = new Parser("'spam'[1]");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(IndexedExpression));

            IndexedExpression iexpr = (IndexedExpression)expression;

            Assert.AreEqual("spam", iexpr.TargetExpression.Evaluate(null));
            Assert.AreEqual(1, iexpr.IndexExpression.Evaluate(null));
        }

        [TestMethod]
        public void CompileSlicedExpression()
        {
            Parser parser = new Parser("'spam'[1:2]");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(SlicedExpression));

            SlicedExpression sexpr = (SlicedExpression)expression;

            Assert.AreEqual("spam", sexpr.TargetExpression.Evaluate(null));
            Slice slice = (Slice)sexpr.SliceExpression.Evaluate(null);
            Assert.IsTrue(slice.Begin.HasValue);
            Assert.IsTrue(slice.End.HasValue);
            Assert.AreEqual(1, slice.Begin.Value);
            Assert.AreEqual(2, slice.End.Value);
        }

        [TestMethod]
        public void CompileSlicedExpressionWithNullEnd()
        {
            Parser parser = new Parser("'spam'[1:]");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(SlicedExpression));

            SlicedExpression sexpr = (SlicedExpression)expression;

            Assert.AreEqual("spam", sexpr.TargetExpression.Evaluate(null));
            Slice slice = (Slice)sexpr.SliceExpression.Evaluate(null);
            Assert.IsTrue(slice.Begin.HasValue);
            Assert.IsFalse(slice.End.HasValue);
            Assert.AreEqual(1, slice.Begin.Value);
        }

        [TestMethod]
        public void CompileSlicedExpressionWithNullBegin()
        {
            Parser parser = new Parser("'spam'[:2]");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(SlicedExpression));

            SlicedExpression sexpr = (SlicedExpression)expression;

            Assert.AreEqual("spam", sexpr.TargetExpression.Evaluate(null));
            Slice slice = (Slice)sexpr.SliceExpression.Evaluate(null);
            Assert.IsFalse(slice.Begin.HasValue);
            Assert.IsTrue(slice.End.HasValue);
            Assert.AreEqual(2, slice.End.Value);
        }

        [TestMethod]
        public void CompileSlicedExpressionWithNullBeginAndEnd()
        {
            Parser parser = new Parser("'spam'[:]");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(SlicedExpression));

            SlicedExpression sexpr = (SlicedExpression)expression;

            Assert.AreEqual("spam", sexpr.TargetExpression.Evaluate(null));
            Slice slice = (Slice)sexpr.SliceExpression.Evaluate(null);
            Assert.IsFalse(slice.Begin.HasValue);
            Assert.IsFalse(slice.End.HasValue);
        }

        [TestMethod]
        public void EvaluateConcatenateStrings()
        {
            Assert.AreEqual("spam", CompileAndEvaluateExpression("'sp' + 'am'"));
        }

        [TestMethod]
        public void EvaluateRepeatString()
        {
            Assert.AreEqual("spamspamspam", CompileAndEvaluateExpression("'spam' * 3"));
        }

        [TestMethod]
        public void CompileCallExpression()
        {
            Parser parser = new Parser("len('spam')");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(CallExpression));
        }

        [TestMethod]
        public void CompileMethodCallExpression()
        {
            Parser parser = new Parser("foo.find('spam')");
            IExpression expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(CallExpression));

            CallExpression mcexpression = (CallExpression)expression;
            Assert.IsNotNull(mcexpression.TargetExpression);
            Assert.IsInstanceOfType(mcexpression.TargetExpression, typeof(AttributeExpression));
            Assert.AreEqual("find", ((AttributeExpression)mcexpression.TargetExpression).Name);
            Assert.IsNotNull(mcexpression.ArgumentExpressions);
            Assert.AreEqual(1, mcexpression.ArgumentExpressions.Count);
            Assert.IsInstanceOfType(mcexpression.ArgumentExpressions[0], typeof(ConstantExpression));
        }

        [TestMethod]
        public void EvaluateMethodCallExpression()
        {
            Assert.AreEqual(1, CompileAndEvaluateExpression("'spam'.find('pa')"));
        }

        [TestMethod]
        public void EvaluateLen()
        {
            Assert.AreEqual(4, CompileAndEvaluateExpression("len('spam')"));
        }

        [TestMethod]
        public void CompileSimpleSetVariable()
        {
            Parser parser = new Parser("a=1");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SetCommand));

            var scommand = (SetCommand)command;
            Assert.AreEqual("a", scommand.Target);
            Assert.IsInstanceOfType(scommand.Expression, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)scommand.Expression).Value);
        }

        [TestMethod]
        public void CompileSimpleDefFunction()
        {
            Parser parser = new Parser("def foo(a, b):\r\n    print(a)\r\n    print(b)");

            var command = parser.CompileCommand();
            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(DefCommand));

            var dcommand = (DefCommand)command;
            Assert.AreEqual("foo", dcommand.Name);
            Assert.IsNotNull(dcommand.ParameterExpressions);
            Assert.AreEqual(2, dcommand.ParameterExpressions.Count);
            Assert.AreEqual("a", dcommand.ParameterExpressions[0].Name);
            Assert.AreEqual("b", dcommand.ParameterExpressions[1].Name);
            Assert.IsNotNull(dcommand.Body);
            Assert.IsInstanceOfType(dcommand.Body, typeof(CompositeCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileSimpleDefFunctionWithDefaultArgumentValues()
        {
            Parser parser = new Parser("def foo(a=1, b=2):\r\n    print(a)\r\n    print(b)");

            var command = parser.CompileCommand();
            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(DefCommand));

            var dcommand = (DefCommand)command;
            Assert.AreEqual("foo", dcommand.Name);
            Assert.IsNotNull(dcommand.ParameterExpressions);
            Assert.AreEqual(2, dcommand.ParameterExpressions.Count);
            Assert.AreEqual("a", dcommand.ParameterExpressions[0].Name);
            Assert.AreEqual("b", dcommand.ParameterExpressions[1].Name);
            Assert.IsNotNull(dcommand.ParameterExpressions[0].DefaultExpression);
            Assert.IsNotNull(dcommand.ParameterExpressions[1].DefaultExpression);
            Assert.IsNotNull(dcommand.Body);
            Assert.IsInstanceOfType(dcommand.Body, typeof(CompositeCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileSimpleDefFunctionWithoutParameters()
        {
            Parser parser = new Parser("def foo():\r\n    pass");

            var command = parser.CompileCommand();
            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(DefCommand));

            var dcommand = (DefCommand)command;
            Assert.AreEqual("foo", dcommand.Name);
            Assert.IsNotNull(dcommand.ParameterExpressions);
            Assert.AreEqual(0, dcommand.ParameterExpressions.Count);
            Assert.IsNotNull(dcommand.Body);

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileTwoSimpleDefFunction()
        {
            Parser parser = new Parser("def foo(a):\r\n    print(a)\r\ndef bar(b):\r\n    print(b)");

            var command = parser.CompileCommand();
            Assert.IsInstanceOfType(command, typeof(DefCommand));
            command = parser.CompileCommand();
            Assert.IsInstanceOfType(command, typeof(DefCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompilePassCommand()
        {
            Parser parser = new Parser("pass");

            var command = parser.CompileCommand();
            Assert.IsInstanceOfType(command, typeof(PassCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileWhileCommandWithSingleCommandSameLine()
        {
            Parser parser = new Parser("while a: print(a)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(WhileCommand));

            WhileCommand whilecmd = (WhileCommand)cmd;

            Assert.IsNotNull(whilecmd.Condition);
            Assert.IsInstanceOfType(whilecmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(whilecmd.Command);
            Assert.IsInstanceOfType(whilecmd.Command, typeof(ExpressionCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileWhileCommandWithCompositeCommandSameLine()
        {
            Parser parser = new Parser("while a: print(a); print(b)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(WhileCommand));

            WhileCommand whilecmd = (WhileCommand)cmd;

            Assert.IsNotNull(whilecmd.Condition);
            Assert.IsInstanceOfType(whilecmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(whilecmd.Command);
            Assert.IsInstanceOfType(whilecmd.Command, typeof(CompositeCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileWhileCommandWithSingleCommand()
        {
            Parser parser = new Parser("while a:\r\n  print(a)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(WhileCommand));

            WhileCommand whilecmd = (WhileCommand)cmd;

            Assert.IsNotNull(whilecmd.Condition);
            Assert.IsInstanceOfType(whilecmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(whilecmd.Command);
            Assert.IsInstanceOfType(whilecmd.Command, typeof(ExpressionCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileWhileCommandWithCompositeCommand()
        {
            Parser parser = new Parser("while a:\r\n  print(a)\r\n  print(b)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(WhileCommand));

            WhileCommand whilecmd = (WhileCommand)cmd;

            Assert.IsNotNull(whilecmd.Condition);
            Assert.IsInstanceOfType(whilecmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(whilecmd.Command);
            Assert.IsInstanceOfType(whilecmd.Command, typeof(CompositeCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileIfCommandWithSingleElseCommandSameLine()
        {
            Parser parser = new Parser("if a:\r\n  print(b)\r\nelse: print(a)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsNotNull(ifcmd.ElseCommand);
            Assert.IsInstanceOfType(ifcmd.ElseCommand, typeof(ExpressionCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileIfCommandWithCompositeElseCommandSameLine()
        {
            Parser parser = new Parser("if a: \r\n  print(c)\r\nelse: print(a); print(b)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsNotNull(ifcmd.ElseCommand);
            Assert.IsInstanceOfType(ifcmd.ElseCommand, typeof(CompositeCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileIfCommandWithSingleElseCommand()
        {
            Parser parser = new Parser("if a:\r\n  print(b)\r\nelse:\r\n  print(a)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsNotNull(ifcmd.ElseCommand);
            Assert.IsInstanceOfType(ifcmd.ElseCommand, typeof(ExpressionCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileIfCommandWithCompositeElseCommand()
        {
            Parser parser = new Parser("if a:\r\n  print(a)\r\nelse:\r\n  print(a)\r\n  print(b)");

            ICommand cmd = parser.CompileCommand();

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(IfCommand));

            IfCommand ifcmd = (IfCommand)cmd;

            Assert.IsNotNull(ifcmd.Condition);
            Assert.IsInstanceOfType(ifcmd.Condition, typeof(NameExpression));
            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsNotNull(ifcmd.ElseCommand);
            Assert.IsInstanceOfType(ifcmd.ElseCommand, typeof(CompositeCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileEmptyReturn()
        {
            Parser parser = new Parser("return");
            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ReturnCommand));

            var retcommand = (ReturnCommand)command;

            Assert.IsNull(retcommand.Expression);
        }

        [TestMethod]
        public void CompileReturnWithValue()
        {
            Parser parser = new Parser("return 1");
            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ReturnCommand));

            var retcommand = (ReturnCommand)command;

            Assert.IsNotNull(retcommand.Expression);
        }

        [TestMethod]
        public void RaiseWhenTwoListArguments()
        {
            Parser parser = new Parser("def foo(*a, *b):");
            
            try 
            {
                parser.CompileCommand();
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(SyntaxError));
                Assert.AreEqual("invalid syntax", ex.Message);
            }
        }

        [TestMethod]
        public void CompileCallWithNamedArgument()
        {
            Parser parser = new Parser("foo(a=1+2)");

            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ExpressionCommand));

            var exprcmd = (ExpressionCommand)command;

            Assert.IsNotNull(exprcmd.Expression);
            Assert.IsInstanceOfType(exprcmd.Expression, typeof(CallExpression));

            var callexpr = (CallExpression)exprcmd.Expression;

            Assert.IsNotNull(callexpr.TargetExpression);
            Assert.IsInstanceOfType(callexpr.TargetExpression, typeof(NameExpression));
            Assert.IsNotNull(callexpr.ArgumentExpressions);
            Assert.AreEqual(1, callexpr.ArgumentExpressions.Count);
            Assert.IsInstanceOfType(callexpr.ArgumentExpressions[0], typeof(NamedArgumentExpression));

            var nexpr = (NamedArgumentExpression)callexpr.ArgumentExpressions[0];
            Assert.AreEqual("a", nexpr.Name);
            Assert.IsNotNull(nexpr.Expression);
        }

        [TestMethod]
        public void CompileSimpleClassDefinition()
        {
            Parser parser = new Parser("class Spam:\r\n  pass");
            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ClassCommand));

            var clscommand = (ClassCommand)command;

            Assert.AreEqual("Spam", clscommand.Name);
            Assert.IsNotNull(clscommand.Body);
            Assert.IsNull(clscommand.BaseExpressions);

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileSimpleClassDefinitionWithSimpleInheritance()
        {
            Parser parser = new Parser("class Spam(object):\r\n  pass");
            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ClassCommand));

            var clscommand = (ClassCommand)command;

            Assert.AreEqual("Spam", clscommand.Name);
            Assert.IsNotNull(clscommand.Body);
            Assert.IsNotNull(clscommand.BaseExpressions);
            Assert.AreEqual(1, clscommand.BaseExpressions.Count);

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileSimpleClassDefinitionWithMultipleInheritance()
        {
            Parser parser = new Parser("class Spam(object,mytype):\r\n  pass");
            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ClassCommand));

            var clscommand = (ClassCommand)command;

            Assert.AreEqual("Spam", clscommand.Name);
            Assert.IsNotNull(clscommand.Body);
            Assert.IsNotNull(clscommand.BaseExpressions);
            Assert.AreEqual(2, clscommand.BaseExpressions.Count);

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileSetAttributeCommand()
        {
            Parser parser = new Parser("adam.name = 'Spam'");
            ICommand command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SetAttributeCommand));

            var setcommand = (SetAttributeCommand)command;

            Assert.AreEqual("name", setcommand.Name);
            Assert.IsInstanceOfType(setcommand.Expression, typeof(ConstantExpression));
            Assert.IsInstanceOfType(setcommand.TargetExpression, typeof(NameExpression));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileSimpleForCommand()
        {
            Parser parser = new Parser("for a in [1,2,3]: b = b +a");

            var command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ForCommand));

            var forcmd = (ForCommand)command;
            Assert.IsNotNull(forcmd.Command);
            Assert.IsInstanceOfType(forcmd.Command, typeof(SetCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileSimpleForMultilineCommand()
        {
            Parser parser = new Parser("for a in [1,2,3]:\r\n  print(a)");

            var command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(ForCommand));

            var forcmd = (ForCommand)command;
            Assert.IsNotNull(forcmd.Command);
            Assert.IsInstanceOfType(forcmd.Command, typeof(ExpressionCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileNegateExpression()
        {
            Parser parser = new Parser("-3");

            var expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(NegateExpression));
        }

        [TestMethod]
        public void CompileNotExpression()
        {
            Parser parser = new Parser("not 3");

            var expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(NotExpression));

            var notexpression = (NotExpression)expression;
            Assert.IsInstanceOfType(notexpression.Expression, typeof(ConstantExpression));
        }

        [TestMethod]
        public void CompileAndExpression()
        {
            Parser parser = new Parser("2 and 3");

            var expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BooleanExpression));

            var booleanexpression = (BooleanExpression)expression;
            Assert.AreEqual(BooleanOperator.And, booleanexpression.Operation);
            Assert.IsInstanceOfType(booleanexpression.Left, typeof(ConstantExpression));
            Assert.IsInstanceOfType(booleanexpression.Right, typeof(ConstantExpression));
        }

        [TestMethod]
        public void CompileOrExpression()
        {
            Parser parser = new Parser("2 or 3");

            var expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BooleanExpression));

            var booleanexpression = (BooleanExpression)expression;
            Assert.AreEqual(BooleanOperator.Or, booleanexpression.Operation);
            Assert.IsInstanceOfType(booleanexpression.Left, typeof(ConstantExpression));
            Assert.IsInstanceOfType(booleanexpression.Right, typeof(ConstantExpression));
        }

        [TestMethod]
        public void CompileBooleanExpression()
        {
            Parser parser = new Parser("2 and 4 or not 3");

            var expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BooleanExpression));

            var booleanexpression = (BooleanExpression)expression;
            Assert.AreEqual(BooleanOperator.Or, booleanexpression.Operation);
            Assert.IsInstanceOfType(booleanexpression.Left, typeof(BooleanExpression));
            Assert.IsInstanceOfType(booleanexpression.Right, typeof(NotExpression));

            var leftexpression = (BooleanExpression)booleanexpression.Left;

            Assert.AreEqual(BooleanOperator.And, leftexpression.Operation);
        }

        [TestMethod]
        public void CompileSetIndexCommand()
        {
            Parser parser = new Parser("a[1] = 2");
            var command = parser.CompileCommand();

            Assert.IsNotNull(command);
            Assert.IsInstanceOfType(command, typeof(SetIndexCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileChainedNamedCalls()
        {
            Parser parser = new Parser("f.foo().bar()");
            var expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(CallExpression));

            var call = (CallExpression)expression;

            Assert.IsNotNull(call.TargetExpression);
            Assert.IsInstanceOfType(call.TargetExpression, typeof(AttributeExpression));

            var attr = (AttributeExpression)call.TargetExpression;

            Assert.IsNotNull(attr.Expression);
            Assert.IsInstanceOfType(attr.Expression, typeof(CallExpression));
        }

        [TestMethod]
        public void CompileChainedCalls()
        {
            Parser parser = new Parser("f.foo()()");
            var expression = parser.CompileExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(CallExpression));

            var call = (CallExpression)expression;

            Assert.IsNotNull(call.TargetExpression);
            Assert.IsInstanceOfType(call.TargetExpression, typeof(CallExpression));

            var call2 = (CallExpression)call.TargetExpression;

            Assert.IsNotNull(call2.TargetExpression);
            Assert.IsInstanceOfType(call2.TargetExpression, typeof(AttributeExpression));
        }

        [TestMethod]
        public void CompileBreakCommand()
        {
            Parser parser = new Parser("break");

            var result = parser.CompileCommand();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BreakCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        [TestMethod]
        public void CompileContinueCommand()
        {
            Parser parser = new Parser("continue");

            var result = parser.CompileCommand();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ContinueCommand));

            Assert.IsNull(parser.CompileCommand());
        }

        private static object CompileAndEvaluateExpression(string text)
        {
            Machine machine = new Machine();
            Parser parser = new Parser(text);

            IExpression expression = parser.CompileExpression();

            Assert.IsNull(parser.CompileExpression());

            return expression.Evaluate(machine.Environment);
        }
    }
}

