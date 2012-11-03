namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class CallExpression : IExpression
    {
        private IExpression targetExpression;
        private IList<IExpression> argumentExpressions;

        public CallExpression(IExpression targetExpression, IList<IExpression> argumentExpressions)
        {
            this.targetExpression = targetExpression;
            this.argumentExpressions = argumentExpressions;
        }

        public IExpression TargetExpression { get { return this.targetExpression; } }

        public IList<IExpression> ArgumentExpressions { get { return this.argumentExpressions; } }

        public object Evaluate(BindingEnvironment environment)
        {
            IFunction function = (IFunction)this.targetExpression.Evaluate(environment);
            IList<object> arguments = null;

            if (this.argumentExpressions != null && this.argumentExpressions.Count > 0)
            {
                arguments = new List<object>();

                foreach (var argexpr in this.argumentExpressions)
                    arguments.Add(argexpr.Evaluate(environment));
            }

            return function.Apply(null, null, arguments);
        }
    }
}
