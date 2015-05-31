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
        private Stack<int> lastChars = new Stack<int>();
        private bool hasChar;
        private int lastIndent = -1;
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

            if (this.lastIndent >= 0)
            {
                indent = this.lastIndent;
                this.lastIndent = -1;
                return indent;
            }

            int ich;

            for (ich = this.NextChar(); ich >= 0 && IsSpace((char)ich); ich = this.NextChar())
                indent++;

            this.PushChar(ich);

            return indent;
        }

        public Token NextToken()
        {
            if (this.tokenStack.Count != 0)
            {
                return this.tokenStack.Pop();
            }

            int ich;
            char ch;

            ich = this.NextCharSkipBlanks();

            if (ich < 0)
                return null;

            ch = (char)ich;

            if (ch == '\n' || ch == '\r')
                return this.NextEndOfLine(ch);

            if (char.IsDigit(ch))
                return this.NextInteger(ch);

            if (char.IsLetter(ch) || ch == '_')
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
                int ich2 = this.NextChar();

                if (ich2 < 0 || (char)ich2 != '\n')
                    this.PushChar(ich2);
                else
                    value += (char)ich2;
            }

            return new Token() { TokenType = TokenType.EndOfLine, Value = value };
        }

        private Token NextOperator(char ch)
        {
            int ich2 = this.NextChar();

            if (ich2 >= 0)
            {
                char ch2;

                ch2 = (char)ich2;

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
                    this.PushChar(ich2);
            }
            else
                this.PushChar(ich2);

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

            int ich = this.NextChar();

            if (ich >= 0)
            {
                char ch = (char)ich;

                if (ch == endchar)
                {
                    int ich2 = this.NextChar();
                    if (ich2 >= 0 && (char)ich2 == endchar)
                        return this.NextMultilineString(endchar);
                    else
                        this.PushChar(ich2);
                }

                while (ich >= 0 && ch != endchar)
                {
                    if (ch == EscapeChar)
                        ch = (char)this.NextChar();

                    sb.Append(ch);
                    ich = this.NextChar();

                    if (ich >= 0)
                        ch = (char)ich;
                }
            }

            Token token = new Token();
            token.Value = sb.ToString();
            token.TokenType = TokenType.String;

            return token;
        }

        private Token NextMultilineString(char endchar)
        {
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                int ich = this.NextChar();

                if (ich < 0)
                {
                    this.PushChar(ich);
                    break;
                }

                char ch = (char)ich;

                if (ch == endchar)
                {
                    int ich2 = this.NextChar();

                    if (ich2 >= 0 && (char)ich2 == endchar)
                    {
                        int ich3 = this.NextChar();

                        if (ich3 >= 0 && (char)ich3 == endchar)
                            break;

                        this.PushChar(ich3);
                    }

                    this.PushChar(ich2);
                }

                if (ch == EscapeChar)
                    ch = (char)this.NextChar();

                sb.Append(ch);
            }

            Token token = new Token();
            token.Value = sb.ToString();
            token.TokenType = TokenType.String;

            return token;
        }

        private Token NextInteger(char ch)
        {
            string integer = ch.ToString();

            int ich = this.NextChar();

            while (ich >= 0 && char.IsDigit((char)ich))
            {
                integer += (char)ich;
                ich = this.NextChar();
            }

            if (ich >=0 && (char)ich == '.') 
                return this.NextReal(integer);

            this.PushChar(ich);

            Token token = new Token();
            token.Value = integer;
            token.TokenType = TokenType.Integer;

            return token;
        }

        private Token NextReal(string integerPart)
        {
            string real = integerPart + ".";
            int ich;

            ich = this.NextChar();

            while (ich >= 0 && char.IsDigit((char)ich))
            {
                real += (char)ich;
                ich = this.NextChar();
            }

            this.PushChar(ich);

            Token token = new Token();
            token.Value = real;
            token.TokenType = TokenType.Real;

            return token;
        }

        private Token NextName(char ch)
        {
            string name = ch.ToString();

            int ich = this.NextChar();

            while (ich >= 0 && char.IsLetterOrDigit((char)ich) || (char)ich == '_')
            {
                name += (char)ich;
                ich = this.NextChar();
            }

            this.PushChar(ich);

            Token token = new Token();
            token.Value = name;
            token.TokenType = TokenType.Name;

            if (name == "true" || name == "false")
                token.TokenType = TokenType.Boolean;

            return token;
        }

        private int NextCharSkipBlanks()
        {
            int ich;

            ich = this.NextChar();

            while (ich >= 0)
            {
                char ch = (char)ich;

                if (!char.IsWhiteSpace(ch) || ch == '\n' || ch == '\r')
                    break;

                ich = this.NextChar();
            }

            return ich;
        }

        private void PushChar(int ch)
        {
            this.lastChars.Push(ch);
        }

        private int NextChar()
        {
            int ich = this.NextSimpleChar();

            if (ich >= 0 && (char)ich == CommentChar)
            {
                while (ich >= 0 && (char)ich != '\r' && (char)ich != '\n')
                    ich = this.NextSimpleChar();
            }

            return ich;
        }

        private int NextSimpleChar()
        {
            if (this.lastChars.Count > 0)
                return this.lastChars.Pop();

            if (this.reader.Equals(System.Console.In) && this.reader.Peek() < 0)
            {
                Console.Out.Write(">>> ");
                Console.Out.Flush();
            }

            return this.reader.Read();
        }
    }
}
