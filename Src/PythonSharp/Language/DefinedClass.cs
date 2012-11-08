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
        private IDictionary<string, IFunction> methods = new Dictionary<string, IFunction>();
        private IDictionary<string, object> values = new Dictionary<string, object>();

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

        public object Apply(BindingEnvironment environment, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            var dynobj = new DynamicObject(this);

            if (this.HasMethod(constructorName))
            {
                IFunction constructor = this.GetMethod(constructorName);
                IList<object> args = new List<object>() { dynobj };

                if (arguments != null && arguments.Count > 0)
                    foreach (var arg in arguments)
                        args.Add(arg);

                constructor.Apply(environment, args, namedArguments);
            }

            return dynobj;
        }

        public object GetValue(string name)
        {
            if (this.methods.ContainsKey(name))
                return this.methods[name];

            if (this.values.ContainsKey(name))
                return this.values[name];

            return null;
        }

        public void SetValue(string name, object value)
        {
            if (value is IFunction)
            {
                this.methods[name] = (IFunction)value;

                if (this.values.ContainsKey(name))
                    this.values.Remove(name);

                return;
            }

            if (this.methods.ContainsKey(name))
                this.methods.Remove(name);

            this.values[name] = value;
        }

        public bool HasValue(string name)
        {
            if (this.methods.ContainsKey(name))
                return true;

            return this.values.ContainsKey(name);
        }
    }
}
