namespace PythonSharp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;

    public class RangeFunction : IFunction
    {
        public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            if (arguments.Count == 1)
                return new Range(Numbers.ToInteger(arguments[0]));

            if (arguments.Count == 2)
                return new Range(Numbers.ToInteger(arguments[0]), Numbers.ToInteger(arguments[1]));

            if (arguments.Count == 3)
                return new Range(Numbers.ToInteger(arguments[0]), Numbers.ToInteger(arguments[1]), Numbers.ToInteger(arguments[2]));

            if (arguments.Count == 0)
                throw new TypeError("range expected 1 arguments, got 0");

            throw new TypeError(string.Format("range expected at most 3 arguments, got {0}", arguments.Count));
        }
    }
}
