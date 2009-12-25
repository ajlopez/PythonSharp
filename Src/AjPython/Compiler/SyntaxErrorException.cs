namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class SyntaxErrorException : ParserException
    {
        public SyntaxErrorException(string message)
            : base(string.Format(CultureInfo.CurrentCulture, "SyntaxError: {0}", message))
        {
        }
    }
}
