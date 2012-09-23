namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class SliceExpression : IExpression
    {
        private IExpression beginExpression;
        private IExpression endExpression;

        public SliceExpression(IExpression beginExpression, IExpression endExpression)
        {
            this.beginExpression = beginExpression;
            this.endExpression = endExpression;
        }

        public IExpression BeginExpression { get { return this.beginExpression; } }

        public IExpression EndExpression { get { return this.endExpression; } }

        public object Evaluate(BindingEnvironment environment)
        {
            return new Slice(this.beginExpression == null ? null : (int?)this.beginExpression.Evaluate(environment), this.endExpression == null ? null : (int?)this.endExpression.Evaluate(environment));
        }
    }
}
