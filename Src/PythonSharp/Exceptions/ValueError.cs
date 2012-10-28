namespace PythonSharp.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ValueError : Exception
    {
        public ValueError(string message)
            : base(message)
        {
        }
    }
}
