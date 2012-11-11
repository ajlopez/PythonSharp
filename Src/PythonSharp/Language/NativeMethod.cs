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

        public delegate object MethodDelegate(IList<object> arguments);

        public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            return this.method(arguments);
        }
    }
}
