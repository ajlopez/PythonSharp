namespace PythonSharp.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SyntaxError : Exception
    {
        public SyntaxError(string message)
            : base(message)
        {
        }
    }
}
