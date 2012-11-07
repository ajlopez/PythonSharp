namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DefinedClass : IType
    {
        private string name;
        private IDictionary<string, IFunction> methods = new Dictionary<string, IFunction>();

        public DefinedClass(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public void SetMethod(string name, IFunction method)
        {
            this.methods[name] = method;
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
    }
}
