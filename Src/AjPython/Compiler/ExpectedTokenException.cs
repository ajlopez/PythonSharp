namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class ExpectedTokenException : CompilerException
    {
        public ExpectedTokenException(string token)
            : base(string.Format(CultureInfo.CurrentCulture, "Expected '{0}'", token))
        {
        }
    }
}
