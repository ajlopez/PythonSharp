namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class UnexpectedEndOfInputException : CompilerException
    {
        public UnexpectedEndOfInputException()
            : base("Unexpected End of Input")
        {
        }
    }
}
