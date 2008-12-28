namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class EndOfInputException : Exception
    {
        public EndOfInputException()
            : base("End of Input")
        {
        }
    }
}
