namespace PythonSharp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;
    using PythonSharp.Compiler;
    using PythonSharp.Expressions;

    public class DirFunction : IFunction
    {
        public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            int nargs = arguments == null ? 0 : arguments.Count;

            if (nargs > 1)
                throw new TypeError(string.Format("dir expected at most 1 arguments, got {0}", nargs));

            IValues values = nargs == 0 ? context : (IValues) arguments[0];

            return values.GetNames().OrderBy(s => s).ToList();
        }
    }
}
