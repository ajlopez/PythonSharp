namespace PythonSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class BindingEnvironment : IContext
    {
        private IContext parent;
        private Dictionary<string, object> values = new Dictionary<string, object>();
        private object returnValue;
        private bool hasReturnValue;

        public BindingEnvironment()
        {
        }

        public BindingEnvironment(IContext parent)
        {
            this.parent = parent;
        }

        public IContext Parent { get { return this.parent; } }

        public IContext GlobalContext
        {
            get
            {
                if (this.parent == null)
                    return this;

                return this.parent.GlobalContext;
            }
        }

        public bool WasContinue { get; set; }

        public bool WasBreak { get; set; }

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
