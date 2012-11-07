namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DynamicObject
    {
        private DefinedClass klass;
        private IDictionary<string, object> values = new Dictionary<string, object>();

        public DynamicObject(DefinedClass klass)
        {
            this.klass = klass;
        }

        public DefinedClass Class { get { return this.klass; } }

        public object GetValue(string name)
        {
            if (this.values.ContainsKey(name))
                return this.values[name];

            return this.klass.GetMethod(name);
        }

        public void SetValue(string name, object value)
        {
            this.values[name] = value;
        }

        public bool HasValue(string name)
        {
            if (this.values.ContainsKey(name))
                return true;

            return this.klass.GetMethod(name) != null;
        }
    }
}
