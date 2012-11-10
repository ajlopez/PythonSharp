namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IFunction
    {
        object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments);
    }
}