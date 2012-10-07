namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NativeMethod : IMethod
    {
        private MethodDelegate method;

        public NativeMethod(MethodDelegate method)
        {
            this.method = method;
        }

        public object Apply(object target, IList<object> arguments)
        {
            return this.method(target, arguments);
        }
    }
}
