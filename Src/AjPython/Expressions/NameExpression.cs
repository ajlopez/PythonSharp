namespace AjPython.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NameExpression : IExpression
    {
        private string name;

        public NameExpression(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public object Evaluate(BindingEnvironment environment)
        {
            object value = environment.GetValue(this.name);

            if (value != null)
                return value;

            if (environment.HasValue(this.name))
                return value;

            throw new InvalidOperationException(string.Format("NameError: name '{0}' not defined", this.name));
        }
    }
}
