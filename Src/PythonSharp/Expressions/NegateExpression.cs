namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class NegateExpression : IExpression
    {
        private IExpression expression;

        public NegateExpression(IExpression expression)
        {
            this.expression = expression;
        }

        public object Evaluate(IContext context)
        {
            return Numbers.Negate(this.expression.Evaluate(context));
        }
    }
}
