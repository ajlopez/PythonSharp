namespace PythonSharp.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;

    public class ExitFunction : IFunction
    {
        public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            int nargs = arguments == null ? 0 : arguments.Count;

            if (nargs > 1)
                throw new TypeError(string.Format("range expected at most 1 arguments, got {0}", nargs));

            int value = 0;

            if (nargs > 0)
                value = Numbers.ToInteger(arguments[0]);

            System.Environment.Exit(value);

            return null;
        }
    }
}
