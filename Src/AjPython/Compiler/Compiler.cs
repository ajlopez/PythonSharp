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
            Expression expression = this.CompileBinaryExpression();

            return expression;
        }

        private Expression CompileBinaryExpression()
        {
            Expression expression = this.CompileTerm();

            if (expression == null)
            {
                return null;
            }

            Token token = this.parser.NextToken();

            while (token != null && token.TokenType == TokenType.Operator)
            {
                Expression expression2 = this.CompileTerm();
                expression = new BinaryOperatorExpression(expression, expression2, this.CompileOperator(token.Value));
                token = this.parser.NextToken();
            }

            if (token != null)
            {
                this.parser.PushToken(token);
            }

            return expression;
        }

        private Operator CompileOperator(string oper)
        {
            if (oper == "+")
            {
                return Operator.Add;
            }

            if (oper == "-")
            {
                return Operator.Substract;
            }

            if (oper == "*")
            {
                return Operator.Multiply;
            }

            if (oper == "/")
            {
                return Operator.Divide;
            }

            throw new System.InvalidOperationException(string.Format("Unexpected {0}", oper));
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
                case TokenType.Integer:
                    return new IntegerExpression(Convert.ToInt32(token.Value));
                case TokenType.Name:
                    return new NameExpression(token.Value);
                case TokenType.Separator:
                    if (token.Value == "(")
                    {
                        Expression expression = this.CompileExpression();
                        this.CompileExpectedToken(")");
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
