namespace PythonSharp.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NameError : Exception
    {
        public NameError(string message)
            : base(message)
        {
        }
    }
}
