namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NativeMethod : IFunction
    {
        private MethodDelegate method;

        public NativeMethod(MethodDelegate method)
        {
            this.method = method;
        }

        public object Apply(BindingEnvironment environment, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            return this.method(arguments);
        }
    }
}
