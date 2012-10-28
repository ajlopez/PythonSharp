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
            this.methods["replace"] = new NativeMethod(ReplaceMethod);
        }

        public IMethod GetMethod(string name)
        {
            return this.methods[name];
        }

        private static int Find(string text, string argument)
        {
            return text.IndexOf(argument);
        }

        private static string Replace(string text, string toreplace, string newtext)
        {
            return text.Replace(toreplace, newtext);
        }

        private static object FindMethod(object target, IList<object> arguments)
        {
            return Find((string)target, (string)arguments[0]);
        }

        private static object ReplaceMethod(object target, IList<object> arguments)
        {
            return Replace((string)target, (string)arguments[0], (string)arguments[1]);
        }
    }
}
