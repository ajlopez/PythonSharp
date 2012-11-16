namespace PythonSharp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Compiler;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Language;
    using PythonSharp.Utilities;

    public class DirFunction : IFunction
    {
        public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            int nargs = arguments == null ? 0 : arguments.Count;

            if (nargs > 1)
                throw new TypeError(string.Format("dir expected at most 1 arguments, got {0}", nargs));

            IValues values = nargs == 0 ? context : arguments[0] as IValues;

            if (nargs == 0 || values != null)
                return values.GetNames().OrderBy(s => s).ToList();

            return ObjectUtilities.GetNames(arguments[0]);
        }
    }
}
