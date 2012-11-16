namespace PythonSharp.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;

    public class IdFunction : IFunction
    {
        public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            int nargs = arguments == null ? 0 : arguments.Count;

            if (nargs != 1)
                throw new TypeError(string.Format("id() takes exactly one argument ({0} given)", nargs));

            object argument = arguments[0];

            if (argument == null)
                return 0;

            return argument.GetHashCode();
        }
    }
}
