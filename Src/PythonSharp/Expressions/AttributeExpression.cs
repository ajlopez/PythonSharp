namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;

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

        public object Evaluate(IContext context)
        {
            var result = this.expression.Evaluate(context);
            IValues values = result as IValues;

            if (values == null)
            {
                IType type = Types.GetType(result);
                return type.GetMethod(this.name);
            }

            object value = values.GetValue(this.name);

            if (value != null)
                return value;

            if (values.HasValue(this.name))
                return value;

            string typename;

            if (values is BindingEnvironment)
                typename = "module";
            else if (values is DynamicObject)
                typename = ((DynamicObject)values).Class.Name;
            else
                typename = values.GetType().Name;

            throw new AttributeError(string.Format("'{1}' object has no attribute '{0}'", this.name, typename));
        }
    }
}
