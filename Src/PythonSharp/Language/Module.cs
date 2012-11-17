namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Module : IContext
    {
        private IDictionary<string, object> values = new Dictionary<string, object>();
        private IContext global;

        public Module(IContext global)
        {
            this.global = global;
        }

        public IContext GlobalContext { get { return this.global; } }

        public object GetValue(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            if (this.global != null)
                return this.global.GetValue(name);

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

            if (this.global != null)
                return this.global.HasValue(name);

            return false;
        }

        public ICollection<string> GetNames()
        {
            return this.values.Keys.ToList();
        }
    }
}
