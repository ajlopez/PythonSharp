namespace PythonSharp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;
    using System.Collections;

    public class LenFunction : IFunction
    {
        public object Apply(IList<object> arguments)
        {
            int nargs = arguments == null ? 0 : arguments.Count;

            if (nargs != 1)
                throw new ArgumentException(string.Format("len() takes exactly one argument ({0} given)", nargs));

            if (arguments[0] is IList)
                return ((IList)arguments[0]).Count;

            return ((string)arguments[0]).Length;
        }
    }
}
