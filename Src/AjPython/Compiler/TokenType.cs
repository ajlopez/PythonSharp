namespace AjPython.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum TokenType
    {
        Name,
        Integer,
        String,
        QuotedString,
        Operator,
        Separator
    }
}
