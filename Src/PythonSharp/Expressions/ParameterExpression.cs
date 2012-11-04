namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class ParameterExpression : IExpression
    {
        private string name;
        private IExpression defaultExpression;
        private bool isList;

        public ParameterExpression(string name, IExpression defaultExpression, bool isList)
        {
            this.name = name;
            this.defaultExpression = defaultExpression;
            this.isList = isList;
        }

        public string Name { get { return this.name; } }

        public IExpression DefaultExpression { get { return this.defaultExpression; } }

        public bool IsList { get { return this.isList; } }

        public object Evaluate(BindingEnvironment environment)
        {
            object defaultValue = null;

            if (this.defaultExpression != null)
                defaultValue = this.defaultExpression.Evaluate(environment);

            return new Parameter(this.name, defaultValue, this.isList);
        }
    }
}
