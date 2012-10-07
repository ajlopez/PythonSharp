namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;

    public class AttributeExpression : IExpression
    {
        private IExpression expression;
        private string name;

        public AttributeExpression(IExpression expression, string name)
        {
            this.expression = expression;
            this.name = name;
        }

        public IExpression Expression { get { return this.expression; } }

        public string Name { get { return this.name; } }

        public object Evaluate(BindingEnvironment environment)
        {
            // TODO it's presume the expression is a binding environmnet
            BindingEnvironment moduleenv = (BindingEnvironment)this.expression.Evaluate(environment);
            object value = moduleenv.GetValue(this.name);

            if (value != null)
                return value;

            if (environment.HasValue(this.name))
                return value;

            throw new AttributeError(string.Format("'module' object has no attribute '{0}'", this.name));
        }
    }
}
