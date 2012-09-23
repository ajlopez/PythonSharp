namespace PythonSharp.Expressions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IndexedExpression : IExpression
    {
        private IExpression targetExpression;
        private IExpression indexExpression;

        public IndexedExpression(IExpression targetExpression, IExpression indexExpression)
        {
            this.targetExpression = targetExpression;
            this.indexExpression = indexExpression;
        }

        public IExpression TargetExpression { get { return this.targetExpression; } }

        public IExpression IndexExpression { get { return this.indexExpression; } }

        public object Evaluate(BindingEnvironment environment)
        {
            object target = this.targetExpression.Evaluate(environment);
            object index = this.indexExpression.Evaluate(environment);

            if (target is string)
                return ((string)target)[(int)index].ToString();

            return ((IList)target)[(int)index];
        }
    }
}
