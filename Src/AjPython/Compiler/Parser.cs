namespace AjPython.Compiler
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AjPython.Commands;
    using AjPython.Expressions;

    public class Parser
    {
        private static string[] opslevel1 = new string[] { "+", "-" };
        private static string[] opslevel2 = new string[] { "*", "/" };
        private static string[] opslevel3 = new string[] { "**" };

        private bool lastSemi;

        private Lexer lexer;

        public Parser(Lexer lexer)
        {
            if (lexer == null)
                throw new System.ArgumentNullException("lexer");

            this.lexer = lexer;
        }

        public Parser(string text)
            : this(new Lexer(text))
        {
        }

        public Parser(TextReader reader)
            : this(new Lexer(reader))
        {
        }

        public IExpression CompileExpression()
        {
            IExpression expression = this.CompileBinaryLevel1Expression();

            return expression;
        }

        public IExpression CompileList()
        {
            ListExpression listExpression = new ListExpression();

            Token token = this.lexer.NextToken();

            while (token != null && token.Value != "]")
            {
                if (listExpression.Expressions.Count != 0)
                {
                    if (token.Value != ",")
                        throw new InvalidDataException(string.Format("Unexpected '{0}'", token.Value));
                }
                else
                    this.lexer.PushToken(token);

                IExpression expression = this.CompileExpression();
                listExpression.Add(expression);

                token = this.lexer.NextToken();
            }

            if (token != null)
                this.lexer.PushToken(token);

            return listExpression;
        }

        public IExpression CompileDictionary()
        {
            DictionaryExpression dictionaryExpression = new DictionaryExpression();

            Token token = this.lexer.NextToken();

            while (token != null && token.Value != "}")
            {
                if (dictionaryExpression.KeyExpressions.Count != 0)
                {
                    if (token.Value != ",")
                        throw new InvalidDataException(string.Format("Unexpected '{0}'", token.Value));
                }
                else
                    this.lexer.PushToken(token);

                IExpression keyExpression = this.CompileExpression();
                this.CompileToken(TokenType.Separator, ":");
                IExpression valueExpression = this.CompileExpression();
                dictionaryExpression.Add(keyExpression, valueExpression);

                token = this.lexer.NextToken();
            }

            if (token != null)
                this.lexer.PushToken(token);

            return dictionaryExpression;
        }

        public ICommand CompileCommand()
        {
            if (!this.lastSemi)
                this.SkipEmptyLines();

            ICommand command = this.CompileSimpleCommand();

            if (command == null)
                return null;

            this.CompileEndOfCommand();

            return command;
        }

        public ICommand CompileCommandList()
        {
            List<ICommand> commands = new List<ICommand>();

            for (ICommand command = this.CompileCommand(); command != null; command = this.CompileCommand())
                commands.Add(command);

            if (commands.Count == 0)
                return null;

            if (commands.Count == 1)
                return commands[0];

            return new CompositeCommand(commands);
        }

        private void CompileEndOfCommand()
        {
            this.lastSemi = false;

            Token token = this.lexer.NextToken();

            if (token == null)
                return;

            if (token.TokenType == TokenType.EndOfLine)
                return;

            if (token.TokenType == TokenType.Separator && token.Value == ";")
            {
                this.lastSemi = true;
                return;
            }

            this.lexer.PushToken(token);

            throw new UnexpectedTokenException(token);
        }

        private ICommand CompileSimpleCommand()
        {
            Token token = this.CompileName();

            if (token == null)
                return null;

            if (token.Value == "print")
            {
                IExpression expression = this.CompileExpression();
                return new PrintCommand(expression);
            }

            if (token.Value == "import")
            {
                Token name = this.CompileName(true);

                return new ImportCommand(name.Value);
            }

            if (token.Value == "from")
            {
                Token name = this.CompileName(true);

                this.CompileName("import");

                IList<string> names = this.CompileNameList();

                return new ImportFromCommand(name.Value, names);
            }

            if (token.Value == "if")
                return this.CompileIfCommand();

            Token token2 = this.lexer.NextToken();

            if (token2 != null && token2.TokenType == TokenType.Operator && token2.Value == "=")
            {
                IExpression expression = this.CompileExpression();
                return new SimpleAssignmentCommand(token.Value, expression);
            }

            if (token2 == null)
                throw new UnexpectedEndOfInputException();

            throw new UnexpectedTokenException(token);
        }

        private IList<string> CompileNameList()
        {
            IList<string> names = new List<string>();

            names.Add(this.CompileName(true).Value);

            while (this.TryCompile(TokenType.Separator, ","))
                names.Add(this.CompileName(true).Value);

            return names;
        }

        private ICommand CompileIfCommand()
        {
            IExpression condition = this.CompileExpression();
            this.CompileToken(TokenType.Separator, ":");
            this.lastSemi = true;
            ICommand command = this.CompileCommand();
            return new IfCommand(condition, command);
        }

        private void SkipEmptyLines()
        {
            Token token;

            if (this.lexer.NextIndent() != 0)
                throw new SyntaxErrorException("invalid syntax");

            for (token = this.lexer.NextToken(); token != null && token.TokenType == TokenType.EndOfLine; token = this.lexer.NextToken())
                if (this.lexer.NextIndent() != 0)
                    throw new SyntaxErrorException("invalid syntax");

            if (token != null)
                this.lexer.PushToken(token);
        }

        private static Operator CompileOperator(string oper)
        {
            if (oper == "+")
                return Operator.Add;

            if (oper == "-")
                return Operator.Subtract;

            if (oper == "*")
                return Operator.Multiply;

            if (oper == "/")
                return Operator.Divide;

            if (oper == "**")
                return Operator.Power;

            throw new System.InvalidOperationException(string.Format("Unexpected {0}", oper));
        }

        private static bool IsLevel1Operator(Token token)
        {
            return token != null && token.TokenType == TokenType.Operator && opslevel1.Contains(token.Value);
        }

        private static bool IsLevel2Operator(Token token)
        {
            return token != null && token.TokenType == TokenType.Operator && opslevel2.Contains(token.Value);
        }

        private static bool IsLevel3Operator(Token token)
        {
            return token != null && token.TokenType == TokenType.Operator && opslevel3.Contains(token.Value);
        }

        private IExpression CompileBinaryLevel3Expression()
        {
            IExpression expression = this.CompileTerm();

            if (expression == null)
                return null;

            Token token = this.lexer.NextToken();

            while (IsLevel3Operator(token))
            {
                IExpression expression2 = this.CompileTerm();
                expression = new BinaryOperatorExpression(expression, expression2, CompileOperator(token.Value));
                token = this.lexer.NextToken();
            }

            if (token != null)
                this.lexer.PushToken(token);

            return expression;
        }

        private IExpression CompileBinaryLevel2Expression()
        {
            IExpression expression = this.CompileBinaryLevel3Expression();

            if (expression == null)
                return null;

            Token token = this.lexer.NextToken();

            while (IsLevel2Operator(token))
            {
                IExpression expression2 = this.CompileBinaryLevel3Expression();
                expression = new BinaryOperatorExpression(expression, expression2, CompileOperator(token.Value));
                token = this.lexer.NextToken();
            }

            if (token != null)
                this.lexer.PushToken(token);

            return expression;
        }

        private IExpression CompileBinaryLevel1Expression()
        {
            IExpression expression = this.CompileBinaryLevel2Expression();

            if (expression == null)
                return null;

            Token token = this.lexer.NextToken();

            while (IsLevel1Operator(token))
            {
                IExpression expression2 = this.CompileBinaryLevel2Expression();
                expression = new BinaryOperatorExpression(expression, expression2, CompileOperator(token.Value));
                token = this.lexer.NextToken();
            }

            if (token != null)
                this.lexer.PushToken(token);

            return expression;
        }

        private IExpression CompileTerm()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return null;

            switch (token.TokenType)
            {
                case TokenType.String:
                    return new ConstantExpression(token.Value);
                case TokenType.Integer:
                    return new ConstantExpression(System.Convert.ToInt32(token.Value));
                case TokenType.Real:
                    return new ConstantExpression(System.Convert.ToDouble(token.Value));
                case TokenType.Boolean:
                    return new ConstantExpression(System.Convert.ToBoolean(token.Value));
                case TokenType.Name:
                    if (!this.TryCompile(TokenType.Operator, "."))
                        return new NameExpression(token.Value);
                    return new QualifiedNameExpression(token.Value, this.CompileName(true).Value);
                case TokenType.Separator:
                    if (token.Value == "(")
                    {
                        IExpression expression = this.CompileExpression();
                        this.CompileToken(TokenType.Separator, ")");
                        return expression;
                    }

                    if (token.Value == "[")
                    {
                        IExpression expression = this.CompileList();
                        this.CompileToken(TokenType.Separator, "]");
                        return expression;
                    }

                    if (token.Value == "{")
                    {
                        IExpression expression = this.CompileDictionary();
                        this.CompileToken(TokenType.Separator, "}");
                        return expression;
                    }

                    break;
            }

            throw new SyntaxErrorException(string.Format("Unknown '{0}'", token.Value));
        }

        private void CompileToken(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.TokenType != type || token.Value != value)
                throw new SyntaxErrorException(string.Format("'{0}' expected", value));
        }

        private Token CompileName()
        {
            return this.CompileName(false);
        }

        private Token CompileName(bool required)
        {
            Token token = this.lexer.NextToken();

            if (token == null && !required)
                return null;

            if (token.TokenType != TokenType.Name)
                throw new NameExpectedException();

            return token;
        }

        private Token CompileName(string expected)
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.TokenType != TokenType.Name)
            {
                if (token != null)
                    this.lexer.PushToken(token);

                throw new ExpectedTokenException(expected);
            }

            return token;
        }

        private bool TryCompile(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return false;

            if (token.TokenType == type && token.Value == value)
                return true;

            this.lexer.PushToken(token);

            return false;
        }
    }
}
