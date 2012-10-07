namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;

    public class QualifiedNameExpression : IExpression
    {
        private string modulename;
        private string name;

        public QualifiedNameExpression(string modulename, string name)
        {
            this.modulename = modulename;
            this.name = name;
        }

        public string ModuleName { get { return this.modulename; } }

        public string Name { get { return this.name; } }

        public object Evaluate(BindingEnvironment environment)
        {
            BindingEnvironment moduleenv = (BindingEnvironment)environment.GetValue(this.modulename);
            object value = moduleenv.GetValue(this.name);

            if (value != null)
                return value;

            if (environment.HasValue(this.name))
                return value;

            throw new AttributeError(string.Format("'module' object has no attribute '{0}'", this.name));
        }
    }
}
