namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DynamicObject
    {
        private DefinedClass klass;

        public DynamicObject(DefinedClass klass)
        {
            this.klass = klass;
        }

        public DefinedClass Class { get { return this.klass; } }

        public object GetValue(string name)
        {
            return this.klass.GetMethod(name);
        }

        public bool HasValue(string name)
        {
            return this.klass.GetMethod(name) != null;
        }
    }
}
