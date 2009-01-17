namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NameExpectedException : CompilerException
    {
        public NameExpectedException()
            : base("A name was expected")
        {
        }
    }
}
