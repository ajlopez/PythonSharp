namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;
    using PythonSharp.Exceptions;

    public class CallExpression : IExpression
    {
        private IExpression targetExpression;
        private IList<IExpression> argumentExpressions;

        public CallExpression(IExpression targetExpression, IList<IExpression> argumentExpressions)
        {
            this.targetExpression = targetExpression;
            this.argumentExpressions = argumentExpressions;

            if (argumentExpressions != null)
            {
                bool hasnamed = false;

                foreach (var argexpr in argumentExpressions)
                    if (argexpr is NamedArgumentExpression)
                        hasnamed = true;
                    else if (hasnamed)
                        throw new SyntaxError("non-keyword arg after keyword arg");
            }
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

            return function.Apply(environment, arguments);
        }
    }
}
