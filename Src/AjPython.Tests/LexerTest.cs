namespace AjPython.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AjPython;
    using AjPython.Compiler;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void CreateWithString()
        {
            Lexer parser = new Lexer("text");

            Assert.IsNotNull(parser);
        }

        [TestMethod]
        public void CreateWithTextReader()
        {
            Lexer parser = new Lexer(new StringReader("text"));

            Assert.IsNotNull(parser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RaiseIfTextIsNull()
        {
            Lexer parser = new Lexer((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RaiseIfTextReaderIsNull()
        {
            Lexer parser = new Lexer((TextReader)null);
        }

        [TestMethod]
        public void ParseOneCharOperators()
        {
            string operators = "+-*/=";
            Lexer parser = new Lexer(operators);

            Token token;

            foreach (char ch in operators)
            {
                token = parser.NextToken();

                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Operator, token.TokenType);
                Assert.AreEqual(ch.ToString(), token.Value);
            }

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseMultiCharOperators()
        {
            string operators = "**";
            string[] otherOperators = new string[] { "**" };

            Lexer parser = new Lexer(operators);

            Token token;

            foreach (string op in otherOperators)
            {
                token = parser.NextToken();

                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Operator, token.TokenType);
                Assert.AreEqual(op, token.Value);
            }

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseSeparators()
        {
            string separators = "()[]{},:";
            Lexer parser = new Lexer(separators);

            Token token;

            foreach (char ch in separators)
            {
                token = parser.NextToken();

                Assert.IsNotNull(token);
                Assert.AreEqual(TokenType.Separator, token.TokenType);
                Assert.AreEqual(ch.ToString(), token.Value);
            }

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseName()
        {
            Lexer parser = new Lexer("name");

            Token token;

            token = parser.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Name, token.TokenType);
            Assert.AreEqual("name", token.Value);

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseNameWithSpaces()
        {
            Lexer parser = new Lexer(" name ");

            Token token;

            token = parser.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Name, token.TokenType);
            Assert.AreEqual("name", token.Value);

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseInteger()
        {
            Lexer parser = new Lexer("123");

            Token token;

            token = parser.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Integer, token.TokenType);
            Assert.AreEqual("123", token.Value);

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseIntegerWithSpaces()
        {
            Lexer parser = new Lexer(" 123 ");

            Token token;

            token = parser.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Integer, token.TokenType);
            Assert.AreEqual("123", token.Value);

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseReal()
        {
            Lexer parser = new Lexer("12.34");

            Token token;

            token = parser.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Real, token.TokenType);
            Assert.AreEqual("12.34", token.Value);

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseBoolean()
        {
            Lexer parser = new Lexer("true false");

            Token token;

            token = parser.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Boolean, token.TokenType);
            Assert.AreEqual("true", token.Value);

            token = parser.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.Boolean, token.TokenType);
            Assert.AreEqual("false", token.Value);

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseString()
        {
            Lexer parser = new Lexer("\"foo\"");

            Token token;

            token = parser.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("foo", token.Value);

            token = parser.NextToken();

            Assert.IsNull(token);
        }

        [TestMethod]
        public void ParseQuotedString()
        {
            Lexer parser = new Lexer("'bar'");

            Token token;

            token = parser.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(TokenType.String, token.TokenType);
            Assert.AreEqual("bar", token.Value);

            token = parser.NextToken();

            Assert.IsNull(token);
        }
    }
}
