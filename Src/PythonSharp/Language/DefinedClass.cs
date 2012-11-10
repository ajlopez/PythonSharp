namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DefinedClass : IType, IFunction, IValues
    {
        private const string constructorName = "__init__";
        private string name;
        private IDictionary<string, object> values = new Dictionary<string, object>();

        public DefinedClass(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public void SetMethod(string name, IFunction method)
        {
            this.values[name] = method;
        }

        public IFunction GetMethod(string name)
        {
            if (this.values.ContainsKey(name))
            {
                var method = this.values[name] as IFunction;
                return method;
            }

            return null;
        }

        public bool HasMethod(string name)
        {
            return this.values.ContainsKey(name) && this.values[name] is IFunction;
        }

        public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            var dynobj = new DynamicObject(this);

            if (this.HasMethod(constructorName))
            {
                IFunction constructor = this.GetMethod(constructorName);
                IList<object> args = new List<object>() { dynobj };

                if (arguments != null && arguments.Count > 0)
                    foreach (var arg in arguments)
                        args.Add(arg);

                constructor.Apply(context, args, namedArguments);
            }

            return dynobj;
        }

        public object GetValue(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            return null;
        }

        public void SetValue(string name, object value)
        {
            this.values[name] = value;
        }

        public bool HasValue(string name)
        {
            return this.values.ContainsKey(name);
        }

        public ICollection<string> GetNames()
        {
            return this.values.Keys.ToList();
        }
    }
}
