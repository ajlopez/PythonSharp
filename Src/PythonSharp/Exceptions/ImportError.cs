namespace PythonSharp.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ImportError : Exception
    {
        public ImportError(string message)
            : base(message)
        {
        }
    }
}
