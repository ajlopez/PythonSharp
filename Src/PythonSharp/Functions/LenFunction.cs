namespace PythonSharp.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class LenFunction : IFunction
    {
        public object Apply(IList<object> arguments)
        {
            int nargs = arguments == null ? 0 : arguments.Count;

            if (nargs != 1)
                throw new ArgumentException(string.Format("len() takes exactly one argument ({0} given)", nargs));

            object argument = arguments[0];

            if (argument is IList)
                return ((IList)argument).Count;

            if (argument is string)
                return ((string)argument).Length;

            return ((ICollection)argument).Count;
        }
    }
}
