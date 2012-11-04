namespace PythonSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BindingEnvironment
    {
        private BindingEnvironment parent;
        private Machine machine;
        private Dictionary<string, object> values = new Dictionary<string, object>();

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
    }
}
