namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class UnexpectedTokenException : SyntaxErrorException
    {
        public UnexpectedTokenException(Token token)
            : base(string.Format(CultureInfo.CurrentCulture, "Unexpected '{0}'", token.Value))
        {
        }

        public UnexpectedTokenException(string text)
            : base(string.Format(CultureInfo.CurrentCulture, "Unexpected '{0}'", text))
        {
        }
    }
}
