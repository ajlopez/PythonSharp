namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DefinedClass : IType
    {
        private string name;
        private IDictionary<string, IMethod> methods = new Dictionary<string, IMethod>();

        public DefinedClass(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public void SetMethod(string name, IMethod method)
        {
            this.methods[name] = method;
        }

        public IMethod GetMethod(string name)
        {
            return this.methods[name];
        }
    }
}
