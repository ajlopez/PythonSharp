namespace PythonSharp.Expressions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class ListExpression : IExpression
    {
        private IList<IExpression> expressions;
        private bool isreadonly;

        public ListExpression(IList<IExpression> expressions)
            : this(expressions, false)
        {
        }

        public ListExpression(IList<IExpression> expressions, bool isreadonly)
        {
            this.expressions = expressions;
            this.isreadonly = isreadonly;
        }

        public bool IsReadOnly { get { return this.isreadonly; } }

        public IList<IExpression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }

        public object Evaluate(IContext context)
        {
            var list = new List<object>();

            foreach (IExpression expression in this.expressions)
                list.Add(expression.Evaluate(context));

            if (this.isreadonly)
                return list.AsReadOnly();

            return list;
        }
    }
}
