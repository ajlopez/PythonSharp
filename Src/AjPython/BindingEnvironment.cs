namespace AjPython
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BindingEnvironment
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public object GetValue(string name)
        {
            if (!this.values.ContainsKey(name))
            {
                return null;
            }

            return this.values[name];
        }

        public void SetValue(string name, object value)
        {
            this.values[name] = value;
        }
    }
}
