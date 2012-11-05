namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IFunction
    {
        object Apply(BindingEnvironment environment, IList<object> arguments, IDictionary<string, object> namedArguments);
    }
}