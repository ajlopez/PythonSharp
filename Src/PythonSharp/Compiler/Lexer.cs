namespace PythonSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Lexer : IDisposable
    {
        private const char StringChar = '"';
        private const char QuotedStringChar = '\'';
        private const char EscapeChar = '\\';
        private const char CommentChar = '#';
        private const string Operators = "+-/*=.><";
        private const string OperatorStarts = "!";
        private const string Separators = "()[]{},:;";

        private static string[] otherOperators = new string[] { "**", "<=", ">=", "==", "<>", "!=" };

        private TextReader reader;
        private char lastChar;
        private bool hasChar;
        private int lastIndent;
        private Stack<Token> tokenStack = new Stack<Token>();

        public Lexer(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            this.reader = new StringReader(text);
        }

        public Lexer(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            this.reader = reader;
        }

        public void PushIndent(int indent)
        {
            this.lastIndent = indent;
        }

        public int NextIndent()
        {
            int indent = 0;

            if (this.lastIndent > 0)
            {
                indent = this.lastIndent;
                this.lastIndent = 0;
                return indent;
            }

            try
            {
                char ch;

                for (ch = this.NextChar(); IsSpace(ch); ch = this.NextChar())
                    indent++;

                this.PushChar(ch);
            }
            catch
            {
            }

            return indent;
        }

        public Token NextToken()
        {
            if (this.tokenStack.Count != 0)
            {
                return this.tokenStack.Pop();
            }

            char ch;

            try
            {
                ch = this.NextCharSkipBlanks();

                if (ch == '\n' || ch == '\r')
                    return this.NextEndOfLine(ch);

                if (char.IsDigit(ch))
                    return this.NextInteger(ch);

                if (char.IsLetter(ch))
                    return this.NextName(ch);

                if (ch == StringChar)
                    return this.NextString(StringChar);

                if (ch == QuotedStringChar)
                    return this.NextString(QuotedStringChar);

                if (Separators.Contains(ch))
                    return this.NextSeparator(ch);

                if (Operators.Contains(ch) || OperatorStarts.Contains(ch))
                    return this.NextOperator(ch);

                throw new InvalidDataException("Unknown input");
            }
            catch (EndOfInputException)
            {
                return null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool dispose)
        {
            if (dispose && this.reader != null)
                this.reader.Dispose();
        }

        internal void PushToken(Token token)
        {
            this.tokenStack.Push(token);
        }

        private static bool IsSpace(char ch)
        {
            return char.IsWhiteSpace(ch) && ch != '\r' && ch != '\n';
        }

        private Token NextEndOfLine(char ch)
        {
            string value = ch.ToString();

            if (ch == '\r')
            {
                try
                {
                    char ch2 = this.NextChar();

                    if (ch2 != '\n')
                        this.PushChar(ch2);
                    else
                        value += ch2;
                }
                catch
                {
                }
            }

            return new Token() { TokenType = TokenType.EndOfLine, Value = value };
        }

        private Token NextOperator(char ch)
        {
            char ch2;

            try
            {
                ch2 = this.NextChar();

                string op = ch.ToString() + ch2.ToString();

                if (otherOperators.Contains(op))
                {
                    return new Token()
                    {
                        TokenType = TokenType.Operator,
                        Value = op
                    };
                }
                else
                    this.PushChar(ch2);
            }
            catch (EndOfInputException)
            {
            }

            if (Operators.Contains(ch)) 
            {
                return new Token()
                {
                    TokenType = TokenType.Operator,
                    Value = ch.ToString()
                };
            }

            throw new InvalidDataException("Unknown input");
        }

        private Token NextSeparator(char ch)
        {
            return new Token()
            {
                TokenType = TokenType.Separator,
                Value = ch.ToString()
            };
        }

        private Token NextString(char endchar)
        {
            StringBuilder sb = new StringBuilder();
            char ch;

            try
            {
                ch = this.NextChar();

                if (ch == endchar)
                {
                    char ch2 = this.NextChar();
                    if (ch2 == endchar)
                        return this.NextMultilineString(endchar);
                    else
                        this.PushChar(ch2);
                }

                while (ch != endchar)
                {
                    if (ch == EscapeChar)
                        ch = this.NextChar();

                    sb.Append(ch);
                    ch = this.NextChar();
                }
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token();
            token.Value = sb.ToString();
            token.TokenType = TokenType.String;

            return token;
        }

        private Token NextMultilineString(char endchar)
        {
            StringBuilder sb = new StringBuilder();
            char ch;

            try
            {
                while (true)
                {
                    ch = this.NextChar();

                    if (ch == endchar)
                    {
                        char ch2 = this.NextChar();
                        if (ch2 == endchar)
                        {
                            char ch3 = this.NextChar();

                            if (ch3 == endchar)
                                break;

                            this.PushChar(ch3);
                            this.PushChar(ch2);
                        }
                    }

                    if (ch == EscapeChar)
                        ch = this.NextChar();

                    sb.Append(ch);
                }
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token();
            token.Value = sb.ToString();
            token.TokenType = TokenType.String;

            return token;
        }

        private Token NextInteger(char ch)
        {
            string integer = ch.ToString();

            try
            {
                ch = this.NextChar();

                while (char.IsDigit(ch))
                {
                    integer += ch;
                    ch = this.NextChar();
                }

                if (ch == '.') 
                    return this.NextReal(integer);

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token();
            token.Value = integer;
            token.TokenType = TokenType.Integer;

            return token;
        }

        private Token NextReal(string integerPart)
        {
            string real = integerPart + ".";
            char ch;

            try
            {
                ch = this.NextChar();

                while (char.IsDigit(ch))
                {
                    real += ch;
                    ch = this.NextChar();
                }

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token();
            token.Value = real;
            token.TokenType = TokenType.Real;

            return token;
        }

        private Token NextName(char ch)
        {
            string name = ch.ToString();

            try
            {
                ch = this.NextChar();

                while (char.IsLetterOrDigit(ch))
                {
                    name += ch;
                    ch = this.NextChar();
                }

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token();
            token.Value = name;
            token.TokenType = TokenType.Name;

            if (name == "true" || name == "false")
                token.TokenType = TokenType.Boolean;

            return token;
        }

        private char NextCharSkipBlanks()
        {
            char ch;

            ch = this.NextChar();

            while (char.IsWhiteSpace(ch) && ch != '\n' && ch != '\r')
                ch = this.NextChar();

            return ch;
        }

        private void PushChar(char ch)
        {
            this.lastChar = ch;
            this.hasChar = true;
        }

        private char NextChar()
        {
            char ch = this.NextSimpleChar();

            if (ch == CommentChar)
            {
                while (ch != '\r' && ch != '\n')
                    ch = this.NextSimpleChar();
            }

            return ch;
        }

        private char NextSimpleChar()
        {
            if (this.hasChar)
            {
                this.hasChar = false;
                return this.lastChar;
            }

            int ch;

            if (this.reader.Equals(System.Console.In) && this.reader.Peek() < 0)
            {
                Console.Out.Write(">>> ");
                Console.Out.Flush();
            }

            ch = this.reader.Read();

            if (ch < 0)
                throw new EndOfInputException();

            return Convert.ToChar(ch);
        }
    }
}
