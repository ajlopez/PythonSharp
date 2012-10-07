namespace PythonSharp.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AttributeError : Exception
    {
        public AttributeError(string message)
            : base(message)
        {
        }
    }
}
