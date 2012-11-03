namespace PythonSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BindingEnvironment
    {
        private BindingEnvironment parent;
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public BindingEnvironment()
        {
        }

        public BindingEnvironment(BindingEnvironment parent)
        {
            this.parent = parent;
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
