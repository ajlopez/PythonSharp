namespace PythonSharp.Tests.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp;
    using PythonSharp.Compiler;

    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void CreateWithString()
        {
            Lexer lexer = new Lexer("text");

            Assert.IsNotNull(lexer);
        }

        [TestMethod]
        public void CreateWithTextReader()
        {
            Lexer lexer = new Lexer(new StringReader("text"));

            Assert.IsNotNull(lexer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RaiseIfTextIsNull()
        {
            Lexer lexer = new Lexer((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RaiseIfTextReaderIsNull()
        {
            Lexer lexer = new Lexer((TextReader)null);
        }

        [TestMethod]
        public void ParseOneCharOperators()
        {
            string operators = "+-*/=.><";
            Lexer lexer = new Lexer(operators);

            Token token;

            foreach (char ch in operators)
            {
                token = lexer.NextToken();

                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Operator, token.TokenType);
                Assert.AreEqual(ch.ToString(), token.Value);
            }

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseMultiCharOperators()
        {
            string operators = "** <= >= <> == !=";
            string[] otherOperators = new string[] { "**", "<=", ">=", "<>", "==", "!=" };

            Lexer lexer = new Lexer(operators);

            Token token;

            foreach (string op in otherOperators)
            {
                token = lexer.NextToken();

                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Operator, token.TokenType);
                Assert.AreEqual(op, token.Value);
            }

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseSeparators()
        {
            string separators = "()[]{},:;";
            Lexer lexer = new Lexer(separators);

            Token token;

            foreach (char ch in separators)
            {
                token = lexer.NextToken();

                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Separator, token.TokenType);
                Assert.AreEqual(ch.ToString(), token.Value);
            }

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseName()
        {
            Lexer lexer = new Lexer("name");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Name, token.TokenType);
            Assert.AreEqual("name", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseNameWithUnderscores()
        {
            Lexer lexer = new Lexer("__init__");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Name, token.TokenType);
            Assert.AreEqual("__init__", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseNameWithSpaces()
        {
            Lexer lexer = new Lexer(" name ");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Name, token.TokenType);
            Assert.AreEqual("name", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseComment()
        {
            Lexer lexer = new Lexer("# this is a comment");

            Token token;

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseNameComment()
        {
            Lexer lexer = new Lexer("name # this is a comment");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Name, token.TokenType);
            Assert.AreEqual("name", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseCommentName()
        {
            Lexer lexer = new Lexer("# this is a comment\nname");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.EndOfLine, token.TokenType);

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Name, token.TokenType);
            Assert.AreEqual("name", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseInteger()
        {
            Lexer lexer = new Lexer("123");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Integer, token.TokenType);
            Assert.AreEqual("123", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseIntegerWithSpaces()
        {
            Lexer lexer = new Lexer(" 123 ");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Integer, token.TokenType);
            Assert.AreEqual("123", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseReal()
        {
            Lexer lexer = new Lexer("12.34");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Real, token.TokenType);
            Assert.AreEqual("12.34", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseBoolean()
        {
            Lexer lexer = new Lexer("true false");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Boolean, token.TokenType);
            Assert.AreEqual("true", token.Value);

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Boolean, token.TokenType);
            Assert.AreEqual("false", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseString()
        {
            Lexer lexer = new Lexer("\"foo\"");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("foo", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseQuotedString()
        {
            Lexer lexer = new Lexer("'bar'");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("bar", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseQuotedStringWithEscapedQuote()
        {
            Lexer lexer = new Lexer("'bar\\''");

            Token token;

            token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("bar'", token.Value);

            token = lexer.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseNewLine()
        {
            Lexer lexer = new Lexer("\n");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.EndOfLine, token.TokenType);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseCarriageReturnNewLine()
        {
            Lexer lexer = new Lexer("\r\n");

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.EndOfLine, token.TokenType);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void NextIndent()
        {
            Assert.AreEqual(0, (new Lexer("foo")).NextIndent());
            Assert.AreEqual(1, (new Lexer(" foo")).NextIndent());
            Assert.AreEqual(2, (new Lexer("  foo")).NextIndent());

            Lexer lexer = new Lexer("   foo");
            Assert.AreEqual(3, lexer.NextIndent());

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Name, token.TokenType);
            Assert.AreEqual("foo", token.Value);
        }

        [TestMethod]
        public void ParseMultilineSingleQuotedString()
        {
            Lexer lexer = new Lexer("'''This is a\r\nmulti''line\r\nstring'''");
            
            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(TokenType.String, result.TokenType);
            Assert.AreEqual("This is a\r\nmulti''line\r\nstring", result.Value);
            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void ParseMultilineDoubleQuotedString()
        {
            Lexer lexer = new Lexer("\"\"\"This is a\r\nmultiline\r\nstring\"\"\"");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual(TokenType.String, result.TokenType);
            Assert.AreEqual("This is a\r\nmultiline\r\nstring", result.Value);
            Assert.IsNull(lexer.NextToken());
        }
    }
}
