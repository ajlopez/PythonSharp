namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public abstract class ParserException : Exception
    {
        protected ParserException(string msg)
            : base(msg)
        {
        }
    }
}
