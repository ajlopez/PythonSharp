namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using AjPython.Nodes;

    public class Compiler
    {
        private static string[] opslevel1 = new string[] { "+", "-" };
        private static string[] opslevel2 = new string[] { "*", "/" };
        private static string[] opslevel3 = new string[] { "**" };

        private Parser parser;

        public Compiler(Parser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }

            this.parser = parser;
        }

        public Compiler(string text)
            : this(new Parser(text))
        {
        }

        public Compiler(TextReader reader)
            : this(new Parser(reader))
        {
        }

        public Expression CompileExpression()
        {
            Expression expression = this.CompileBinaryLevel1Expression();

            return expression;
        }

        public Expression CompileList()
        {
            ListExpression listExpression = new ListExpression();

            Token token = this.parser.NextToken();

            while (token != null && token.Value != "]")
            {
                if (listExpression.Expressions.Count != 0)
                {
                    if (token.Value != ",")
                    {
                        throw new InvalidDataException(string.Format("Unexpected '{0}'", token.Value));
                    }
                }
                else
                {
                    this.parser.PushToken(token);
                }

                Expression expression = this.CompileExpression();
                listExpression.Add(expression);

                token = parser.NextToken();
            }

            if (token != null)
                this.parser.PushToken(token);

            return listExpression;
        }

        public Expression CompileDictionary()
        {
            DictionaryExpression dictionaryExpression = new DictionaryExpression();

            Token token = this.parser.NextToken();

            while (token != null && token.Value != "}")
            {
                if (dictionaryExpression.KeyExpressions.Count != 0)
                {
                    if (token.Value != ",")
                    {
                        throw new InvalidDataException(string.Format("Unexpected '{0}'", token.Value));
                    }
                }
                else
                {
                    this.parser.PushToken(token);
                }

                Expression keyExpression = this.CompileExpression();
                this.CompileExpectedToken(":");
                Expression valueExpression = this.CompileExpression();
                dictionaryExpression.Add(keyExpression, valueExpression);

                token = parser.NextToken();
            }

            if (token != null)
                this.parser.PushToken(token);

            return dictionaryExpression;
        }

        private static Operator CompileOperator(string oper)
        {
            if (oper == "+")
            {
                return Operator.Add;
            }

            if (oper == "-")
            {
                return Operator.Subtract;
            }

            if (oper == "*")
            {
                return Operator.Multiply;
            }

            if (oper == "/")
            {
                return Operator.Divide;
            }

            if (oper == "**")
            {
                return Operator.Power;
            }

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

        private Expression CompileBinaryLevel3Expression()
        {
            Expression expression = this.CompileTerm();

            if (expression == null)
            {
                return null;
            }

            Token token = this.parser.NextToken();

            while (IsLevel3Operator(token))
            {
                Expression expression2 = this.CompileTerm();
                expression = new BinaryOperatorExpression(expression, expression2, CompileOperator(token.Value));
                token = this.parser.NextToken();
            }

            if (token != null)
            {
                this.parser.PushToken(token);
            }

            return expression;
        }

        private Expression CompileBinaryLevel2Expression()
        {
            Expression expression = this.CompileBinaryLevel3Expression();

            if (expression == null)
            {
                return null;
            }

            Token token = this.parser.NextToken();

            while (IsLevel2Operator(token))
            {
                Expression expression2 = this.CompileBinaryLevel3Expression();
                expression = new BinaryOperatorExpression(expression, expression2, CompileOperator(token.Value));
                token = this.parser.NextToken();
            }

            if (token != null)
            {
                this.parser.PushToken(token);
            }

            return expression;
        }

        private Expression CompileBinaryLevel1Expression()
        {
            Expression expression = this.CompileBinaryLevel2Expression();

            if (expression == null)
            {
                return null;
            }

            Token token = this.parser.NextToken();

            while (IsLevel1Operator(token))
            {
                Expression expression2 = this.CompileBinaryLevel2Expression();
                expression = new BinaryOperatorExpression(expression, expression2, CompileOperator(token.Value));
                token = this.parser.NextToken();
            }

            if (token != null)
            {
                this.parser.PushToken(token);
            }

            return expression;
        }

        private Expression CompileTerm()
        {
            Token token = this.parser.NextToken();

            if (token == null)
            {
                return null;
            }

            switch (token.TokenType)
            {
                case TokenType.String:
                    return new StringExpression(token.Value);
                case TokenType.QuotedString:
                    return new QuotedStringExpression(token.Value);
                case TokenType.Integer:
                    return new IntegerExpression(Convert.ToInt32(token.Value));
                case TokenType.Real:
                    return new RealExpression(Convert.ToDouble(token.Value));
                case TokenType.Boolean:
                    return new BooleanExpression(Convert.ToBoolean(token.Value));
                case TokenType.Name:
                    return new NameExpression(token.Value);
                case TokenType.Separator:
                    if (token.Value == "(")
                    {
                        Expression expression = this.CompileExpression();
                        this.CompileExpectedToken(")");
                        return expression;
                    }
                    if (token.Value == "[")
                    {
                        Expression expression = this.CompileList();
                        this.CompileExpectedToken("]");
                        return expression;
                    }
                    if (token.Value == "{")
                    {
                        Expression expression = this.CompileDictionary();
                        this.CompileExpectedToken("}");
                        return expression;
                    }

                    break;
            }

            throw new InvalidDataException(string.Format("Unknown '{0}'", token.Value));
        }

        private void CompileExpectedToken(string value)
        {
            Token token = this.parser.NextToken();

            if (token == null || token.Value != value)
            {
                throw new InvalidDataException(string.Format("{0} expected", value));
            }
        }
    }
}
