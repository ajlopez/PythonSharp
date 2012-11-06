namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class MethodCallExpression : IExpression
    {
        private IExpression targetExpression;
        private string methodName;
        private IList<IExpression> argumentExpressions;

        public MethodCallExpression(IExpression targetExpression, string methodName, IList<IExpression> argumentExpressions)
        {
            this.targetExpression = targetExpression;
            this.methodName = methodName;
            this.argumentExpressions = argumentExpressions;
        }

        public IExpression TargetExpression { get { return this.targetExpression; } }

        public string MethodName { get { return this.methodName; } }

        public IList<IExpression> ArgumentExpressions { get { return this.argumentExpressions; } }

        public object Evaluate(BindingEnvironment environment)
        {
            object target = this.targetExpression.Evaluate(environment);
            IType type = Types.GetType(target);
            IFunction method = type.GetMethod(this.methodName);
            IList<object> arguments = new List<object>();
            arguments.Add(target);

            if (this.argumentExpressions != null && this.argumentExpressions.Count > 0)
            {
                foreach (var argexpr in this.argumentExpressions)
                    arguments.Add(argexpr.Evaluate(environment));
            }

            return method.Apply(environment, arguments, null);
        }
    }
}
