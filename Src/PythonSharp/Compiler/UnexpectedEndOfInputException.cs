namespace PythonSharp.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;

    public class UnexpectedEndOfInputException : SyntaxError
    {
        public UnexpectedEndOfInputException()
            : base("Unexpected End of Input")
        {
        }
    }
}
