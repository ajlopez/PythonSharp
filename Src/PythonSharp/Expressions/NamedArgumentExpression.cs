namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NamedArgumentExpression : IExpression
    {
        private string name;
        private IExpression expression;

        public NamedArgumentExpression(string name, IExpression expression)
        {
            this.name = name;
            this.expression = expression;
        }

        public string Name { get { return this.name; } }

        public IExpression Expression { get { return this.expression; } }

        public object Evaluate(BindingEnvironment environment)
        {
            return this.expression.Evaluate(environment);
        }
    }
}
