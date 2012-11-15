namespace PythonSharp.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;

    public class StringType : IType
    {
        private IDictionary<string, IFunction> methods = new Dictionary<string, IFunction>();

        public StringType()
        {
            this.methods["find"] = new NativeMethod(FindMethod);
            this.methods["replace"] = new NativeMethod(ReplaceMethod);
            this.methods["split"] = new NativeMethod(SplitMethod);
            this.methods["join"] = new NativeMethod(JoinMethod);
        }

        public IFunction GetMethod(string name)
        {
            if (this.methods.ContainsKey(name))
                return this.methods[name];

            return null;
        }

        public bool HasMethod(string name)
        {
            return this.methods.ContainsKey(name);
        }

        private static int Find(string text, string argument)
        {
            return text.IndexOf(argument);
        }

        private static string Replace(string text, string toreplace, string newtext)
        {
            return text.Replace(toreplace, newtext);
        }

        private static string Join(string sep, IList objects)
        {
            var result = string.Empty;
            var nobjects = 0;

            foreach (var obj in objects)
            {
                if (nobjects > 0)
                    result += sep;

                result += obj;
                nobjects++;
            }

            return result;
        }

        private static string[] Split(string text, string separator)
        {
            if (separator == null)
                return new string[] { text };

            if (string.IsNullOrEmpty(separator))
                throw new ValueError("empty separator");

            IList<string> result = new List<string>();

            for (int position = text.IndexOf(separator); position >= 0; position = text.IndexOf(separator))
            {
                result.Add(text.Substring(0, position));
                text = text.Substring(position + separator.Length);
            }

            result.Add(text);

            return result.ToArray();
        }

        private static object FindMethod(IList<object> arguments)
        {
            return Find((string)arguments[0], (string)arguments[1]);
        }

        private static object ReplaceMethod(IList<object> arguments)
        {
            return Replace((string)arguments[0], (string)arguments[1], (string)arguments[2]);
        }

        private static object SplitMethod(IList<object> arguments)
        {
            return Split((string)arguments[0], (string)arguments[1]);
        }

        private static object JoinMethod(IList<object> arguments)
        {
            return Join((string)arguments[0], (IList)arguments[1]);
        }
    }
}
