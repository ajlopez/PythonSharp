namespace PythonSharp.Compiler
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using PythonSharp.Commands;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    public class Parser
    {
        private static string[] opslevel0 = new string[] { ">", "<", ">=", "<=", "<>", "==", "!=" };
        private static string[] opslevel1 = new string[] { "+", "-" };
        private static string[] opslevel2 = new string[] { "*", "/" };
        private static string[] opslevel3 = new string[] { "**" };
        private static Token endOfLineToken = new Token() { TokenType = TokenType.EndOfLine, Value = "\r\n" };

        private bool lastSemi;
        private int indent;

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
            IExpression expression = this.CompileBinaryLevel0Expression();

            return expression;
        }

        public IList<IExpression> CompileExpressionList()
        {
            IList<IExpression> expressions = new List<IExpression>();

            IExpression expression = this.CompileExpression();

            if (expression == null)
                return null;

            expressions.Add(expression);

            while (this.TryCompile(TokenType.Separator, ","))
                expressions.Add(this.CompileExpression());

            return expressions;
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
                        throw new ExpectedTokenException(",");
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
                        throw new ExpectedTokenException(",");
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
            {
                if (this.indent == 0)
                    this.SkipEmptyLines();

                int newindent = this.lexer.NextIndent();

                if (newindent < this.indent)
                {
                    this.lexer.PushIndent(newindent);
                    return null;
                }

                if (newindent > this.indent)
                    throw new SyntaxError("unexpected indent");
            }

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

        public ICommand CompileNestedCommandList(int newindent)
        {
            int oldindent = this.indent;

            try
            {
                this.indent = newindent;

                List<ICommand> commands = new List<ICommand>();

                this.lexer.PushIndent(newindent);

                for (ICommand command = this.CompileCommand(); command != null; command = this.CompileCommand())
                    commands.Add(command);

                this.lexer.PushToken(endOfLineToken);

                if (commands.Count == 0)
                    return null;

                if (commands.Count == 1)
                    return commands[0];

                return new CompositeCommand(commands);
            }
            finally
            {
                this.indent = oldindent;
            }
        }

        private static BinaryOperator CompileOperator(string oper)
        {
            if (oper == "+")
                return BinaryOperator.Add;

            if (oper == "-")
                return BinaryOperator.Subtract;

            if (oper == "*")
                return BinaryOperator.Multiply;

            if (oper == "/")
                return BinaryOperator.Divide;

            if (oper == "**")
                return BinaryOperator.Power;

            throw new System.InvalidOperationException(string.Format("Unexpected {0}", oper));
        }

        private static ComparisonOperator CompileCompareOperator(string oper)
        {
            if (oper == "<")
                return ComparisonOperator.Less;

            if (oper == "<=")
                return ComparisonOperator.LessEqual;

            if (oper == ">")
                return ComparisonOperator.Greater;

            if (oper == ">=")
                return ComparisonOperator.GreaterEqual;

            if (oper == "==")
                return ComparisonOperator.Equal;

            if (oper == "<>" || oper == "!=")
                return ComparisonOperator.NotEqual;

            throw new System.InvalidOperationException(string.Format("Unexpected {0}", oper));
        }

        private static bool IsLevel0Operator(Token token)
        {
            return token != null && token.TokenType == TokenType.Operator && opslevel0.Contains(token.Value);
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

        private bool TryPeekCompileEndOfCommand()
        {
            Token token = this.lexer.NextToken();
            this.lexer.PushToken(token);

            if (token == null || token.TokenType == TokenType.EndOfLine)
                return true;

            if (token.TokenType == TokenType.Separator && token.Value == ";")
                return true;

            return false;
        }

        private ICommand CompileExpressionCommand()
        {
            IExpression expr = this.CompileExpression();

            if (expr == null)
                return null;

            ListExpression lexpr = null;

            while (this.TryCompile(TokenType.Separator, ","))
            {
                if (lexpr == null)
                {
                    lexpr = new ListExpression();
                    lexpr.Add(expr);
                }

                lexpr.Add(this.CompileExpression());
            }

            if (lexpr != null)
                return new ExpressionCommand(lexpr);

            return new ExpressionCommand(expr);
        }

        private ICommand CompileSimpleCommand()
        {
            Token token = this.TryCompile(TokenType.Name);
            IExpression expr;

            if (token == null)
                return this.CompileExpressionCommand();

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

            if (token.Value == "class")
                return this.CompileClassCommand();

            if (token.Value == "while")
                return this.CompileWhileCommand();

            if (token.Value == "def")
                return this.CompileDefCommand();

            if (token.Value == "pass")
                return new PassCommand();

            if (token.Value == "return")
                return this.CompileReturnCommand();

            Token token2 = this.lexer.NextToken();

            if (token2 != null && token2.TokenType == TokenType.Operator && token2.Value == "=")
            {
                expr = this.CompileExpression();
                return new SetCommand(token.Value, expr);
            }

            if (token2 != null)
                this.lexer.PushToken(token2);

            this.lexer.PushToken(token);

            return this.CompileExpressionCommand();
        }

        private IList<string> CompileNameList()
        {
            IList<string> names = new List<string>();

            names.Add(this.CompileName(true).Value);

            while (this.TryCompile(TokenType.Separator, ","))
                names.Add(this.CompileName(true).Value);

            return names;
        }

        private IList<ParameterExpression> CompileParameterExpressionList()
        {
            IList<ParameterExpression> parameters = new List<ParameterExpression>();

            if (this.TryPeekCompile(TokenType.Separator, ")"))
                return parameters;

            ParameterExpression parexpr = this.CompileParameterExpression();

            bool haslist = parexpr.IsList;

            parameters.Add(parexpr);

            while (this.TryCompile(TokenType.Separator, ","))
            {
                parexpr = this.CompileParameterExpression();
                if (parexpr.IsList)
                    if (haslist)
                        throw new SyntaxError("invalid syntax");
                    else
                        haslist = true;

                parameters.Add(parexpr);
            }

            return parameters;
        }

        private ParameterExpression CompileParameterExpression()
        {
            bool isList = this.TryCompile(TokenType.Operator, "*");
            string name = this.CompileName(true).Value;
            IExpression expression = null;

            if (this.TryCompile(TokenType.Operator, "="))
                expression = this.CompileExpression();

            return new ParameterExpression(name, expression, isList);
        }

        private ICommand CompileReturnCommand()
        {
            if (this.TryPeekCompileEndOfCommand())
                return new ReturnCommand(null);

            return new ReturnCommand(this.CompileExpression());
        }

        private ICommand CompileIfCommand()
        {
            IExpression condition = this.CompileExpression();
            ICommand thencommand;

            this.CompileToken(TokenType.Separator, ":");

            Token token = this.lexer.NextToken();

            if (token == null)
                throw new UnexpectedEndOfInputException();

            if (token.TokenType != TokenType.EndOfLine)
            {
                this.lexer.PushToken(token);
                this.lastSemi = true;
                thencommand = this.CompileCommandList();
            }
            else
            {
                int newindent = this.lexer.NextIndent();
                thencommand = this.CompileNestedCommandList(newindent);
            }

            token = this.lexer.NextToken();

            if (token != null && token.TokenType == TokenType.EndOfLine)
            {
                int indent = this.lexer.NextIndent();

                if (indent != this.indent)
                {
                    this.lexer.PushIndent(indent);
                }
                else if (this.TryCompile(TokenType.Name, "else"))
                {
                    ICommand elsecommand;

                    this.CompileToken(TokenType.Separator, ":");

                    token = this.lexer.NextToken();

                    if (token == null)
                        throw new UnexpectedEndOfInputException();

                    if (token.TokenType != TokenType.EndOfLine)
                    {
                        this.lexer.PushToken(token);
                        this.lastSemi = true;
                        elsecommand = this.CompileCommandList();
                    }
                    else
                    {
                        int newindent = this.lexer.NextIndent();
                        elsecommand = this.CompileNestedCommandList(newindent);
                    }

                    return new IfCommand(condition, thencommand, elsecommand);
                }
                else
                    this.lexer.PushToken(token);
            }

            return new IfCommand(condition, thencommand);
        }

        private ICommand CompileWhileCommand()
        {
            IExpression condition = this.CompileExpression();
            ICommand command;

            this.CompileToken(TokenType.Separator, ":");

            Token token = this.lexer.NextToken();

            if (token == null)
                throw new UnexpectedEndOfInputException();

            if (token.TokenType != TokenType.EndOfLine)
            {
                this.lexer.PushToken(token);
                this.lastSemi = true;
                command = this.CompileCommandList();
            }
            else
            {
                int newindent = this.lexer.NextIndent();
                command = this.CompileNestedCommandList(newindent);
            }

            return new WhileCommand(condition, command);
        }

        private ICommand CompileDefCommand()
        {
            Token token = this.CompileName(true);
            string name = token.Value;
            this.CompileToken(TokenType.Separator, "(");
            IList<ParameterExpression> parameters = this.CompileParameterExpressionList();
            this.CompileToken(TokenType.Separator, ")");

            this.CompileToken(TokenType.Separator, ":");
            this.CompileToken(TokenType.EndOfLine);

            int newindent = this.lexer.NextIndent();
            ICommand body = this.CompileNestedCommandList(newindent);

            return new DefCommand(name, parameters, body);
        }

        private ICommand CompileClassCommand()
        {
            string name = this.CompileName(true).Value;
            this.CompileToken(TokenType.Separator, ":");
            this.CompileToken(TokenType.EndOfLine);
            int newindent = this.lexer.NextIndent();
            ICommand body = this.CompileNestedCommandList(newindent);
            return new ClassCommand(name, body);
        }

        private void SkipEmptyLines()
        {
            Token token;
            int newindent;

            while (true)
            {
                newindent = this.lexer.NextIndent();

                //if (newindent != 0)
                //    throw new SyntaxErrorException("invalid syntax");

                token = this.lexer.NextToken();

                if (token == null)
                    return;

                if (token.TokenType != TokenType.EndOfLine)
                {
                    this.lexer.PushToken(token);
                    this.lexer.PushIndent(newindent);
                    return;
                }
            }
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

        private IExpression CompileBinaryLevel0Expression()
        {
            IExpression expression = this.CompileBinaryLevel1Expression();

            if (expression == null)
                return null;

            Token token = this.lexer.NextToken();

            while (IsLevel0Operator(token))
            {
                IExpression expression2 = this.CompileBinaryLevel1Expression();
                expression = new CompareExpression(CompileCompareOperator(token.Value), expression, expression2);
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

        private IList<IExpression> CompileArgumentExpressionList()
        {
            IList<IExpression> expressions = new List<IExpression>();

            IExpression expression = this.CompileArgumentExpression();

            if (expression == null)
                return null;

            expressions.Add(expression);

            while (this.TryCompile(TokenType.Separator, ","))
                expressions.Add(this.CompileArgumentExpression());

            return expressions;
        }

        private IExpression CompileArgumentExpression()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return null;

            if (token.TokenType == TokenType.Name)
            {
                Token token2 = this.lexer.NextToken();

                if (token2 != null)
                    if (token2.TokenType == TokenType.Operator && token2.Value == "=")
                        return new NamedArgumentExpression(token.Value, this.CompileExpression());
                    else
                        this.lexer.PushToken(token2);
            }

            this.lexer.PushToken(token);

            return this.CompileExpression();
        }

        private IExpression CompileTerm()
        {
            IExpression term = this.CompileSimpleTerm();

            if (term == null)
                return null;

            while (this.TryCompile(TokenType.Operator, "."))
                term = new AttributeExpression(term, this.CompileName(true).Value);

            Token token = this.lexer.NextToken();

            if (token == null)
                return term;

            if (token.TokenType == TokenType.Separator && token.Value == "(")
            {
                IList<IExpression> expressions = this.CompileArgumentExpressionList();
                this.CompileToken(TokenType.Separator, ")");

                if (term is AttributeExpression) 
                {
                    AttributeExpression attrexpr = (AttributeExpression)term;
                    return new MethodCallExpression(attrexpr.Expression, attrexpr.Name, expressions);
                }

                return new CallExpression(term, expressions);
            }

            if (token.TokenType != TokenType.Separator || token.Value != "[") 
            {
                this.lexer.PushToken(token);
                return term;
            }

            IExpression indexpr = null;
            IExpression endexpr = null;

            if (!this.TryCompile(TokenType.Separator, ":"))
            {
                indexpr = this.CompileExpression();

                if (!this.TryCompile(TokenType.Separator, ":"))
                {
                    this.CompileToken(TokenType.Separator, "]");

                    if (indexpr != null)
                        return new IndexedExpression(term, indexpr);

                    return new SlicedExpression(term, new SliceExpression(null, null));
                }

                if (this.TryCompile(TokenType.Separator, "]"))
                    return new SlicedExpression(term, new SliceExpression(indexpr, endexpr));

                endexpr = this.CompileExpression();
            }
            else
                endexpr = this.CompileExpression();

            this.CompileToken(TokenType.Separator, "]");

            SliceExpression sexpr = new SliceExpression(indexpr, endexpr);
            return new SlicedExpression(term, sexpr);
        }

        private IExpression CompileSimpleTerm()
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
                    return new ConstantExpression(System.Convert.ToDouble(token.Value, System.Globalization.CultureInfo.InvariantCulture));
                case TokenType.Boolean:
                    return new ConstantExpression(System.Convert.ToBoolean(token.Value));
                case TokenType.Name:
                    return this.MakeName(token.Value);
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

            this.lexer.PushToken(token);

            return null;
        }

        private void CompileToken(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.TokenType != type || token.Value != value)
                throw new ExpectedTokenException(value);
        }

        private void CompileToken(TokenType type)
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.TokenType != type)
                throw new ExpectedTokenException(type.ToString());
        }

        private IExpression MakeName(string name)
        {
            if (name == "None")
                return new ConstantExpression(null);

            return new NameExpression(name);
        }

        private Token CompileName(bool required)
        {
            Token token = this.lexer.NextToken();

            if (token == null && !required)
                return null;

            if (token == null || token.TokenType != TokenType.Name)
                throw new NameExpectedException();

            return token;
        }

        private Token CompileName(string expected)
        {
            Token token = this.lexer.NextToken();

            if (token == null || token.TokenType != TokenType.Name || token.Value != expected)
                throw new ExpectedTokenException(expected);

            return token;
        }

        private bool TryPeekCompile(TokenType type, string value)
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return false;

            this.lexer.PushToken(token);

            if (token.TokenType == type && token.Value == value)
                return true;

            return false;
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

        private Token TryCompile(TokenType type)
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return null;

            if (token.TokenType == type)
                return token;

            this.lexer.PushToken(token);

            return null;
        }
    }
}
