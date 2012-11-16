namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DefinedClass : IType, IFunction, IContext
    {
        private const string ConstructorName = "__init__";
        private string name;
        private IContext global;
        private IList<IType> bases;
        private IDictionary<string, object> values = new Dictionary<string, object>();

        public DefinedClass(string name)
            : this(name, null, null)
        {
        }

        public DefinedClass(string name, IContext global)
            : this(name, null, global)
        {
        }

        public DefinedClass(string name, IList<IType> bases)
            : this(name, bases, null)
        {
        }

        public DefinedClass(string name, IList<IType> bases, IContext global)
        {
            this.name = name;
            this.global = global;
            this.bases = bases;
        }

        public string Name { get { return this.name; } }

        public IContext GlobalContext { get { return this.global; } }

        public IList<IType> Bases { get { return this.bases; } }

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

            if (this.bases != null)
                foreach (var type in this.bases) 
                {
                    var method = type.GetMethod(name);

                    if (method != null)
                        return method;
                }

            return null;
        }

        public bool HasMethod(string name)
        {
            bool hasmethod = this.values.ContainsKey(name) && this.values[name] is IFunction;

            if (hasmethod)
                return true;

            if (this.bases != null)
                foreach (var type in this.bases)
                    if (type.HasMethod(name))
                        return true;

            return false;
        }

        public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            var dynobj = new DynamicObject(this);

            if (this.HasMethod(ConstructorName))
            {
                IFunction constructor = this.GetMethod(ConstructorName);
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

            if (this.bases != null)
                foreach (var type in this.bases)
                    if (type.HasValue(name))
                        return type.GetValue(name);

            return null;
        }

        public void SetValue(string name, object value)
        {
            this.values[name] = value;
        }

        public bool HasValue(string name)
        {
            if (this.values.ContainsKey(name))
                return true;

            if (this.bases != null)
                foreach (var type in this.bases)
                    if (type.HasValue(name))
                        return true;

            return false;
        }

        public ICollection<string> GetNames()
        {
            return this.values.Keys.ToList();
        }

        public override string ToString()
        {
            // TODO add id?
            return string.Format("<class '{0}'>", this.name);
        }
    }
}
