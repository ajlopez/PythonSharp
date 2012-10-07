namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StringType : IType
    {
        private IDictionary<string, IMethod> methods = new Dictionary<string, IMethod>();

        public StringType()
        {
            this.methods["find"] = new NativeMethod(FindMethod);
        }

        public IMethod GetMethod(string name)
        {
            return this.methods[name];
        }

        private static int Find(string text, string argument)
        {
            return text.IndexOf(argument);
        }

        private static object FindMethod(object target, IList<object> arguments)
        {
            return Find((string)target, (string)arguments[0]);
        }
    }
}
