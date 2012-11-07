namespace PythonSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class BindingEnvironment : IValues
    {
        private BindingEnvironment parent;
        private Machine machine;
        private Dictionary<string, object> values = new Dictionary<string, object>();
        private object returnValue;
        private bool hasReturnValue;

        public BindingEnvironment()
        {
        }

        public BindingEnvironment(Machine machine)
        {
            this.machine = machine;
        }

        public BindingEnvironment(BindingEnvironment parent)
        {
            this.parent = parent;
        }

        public BindingEnvironment Parent { get { return this.parent; } }

        public Machine Machine
        {
            get
            {
                if (this.machine != null)
                    return this.machine;
                if (this.parent != null)
                    return this.parent.Machine;
                return null;
            }
        }

        public bool HasReturnValue()
        {
            return this.hasReturnValue;
        }

        public object GetReturnValue()
        {
            return this.returnValue;
        }

        public void SetReturnValue(object value)
        {
            this.returnValue = value;
            this.hasReturnValue = true;
        }

        public object GetValue(string name)
        {
            if (!this.values.ContainsKey(name))
            {
                if (this.parent != null)
                    return this.parent.GetValue(name);
                return null;
            }

            return this.values[name];
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
