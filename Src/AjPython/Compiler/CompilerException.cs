namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public abstract class CompilerException : Exception
    {
        protected CompilerException(string msg)
            : base(msg)
        {
        }
    }
}
