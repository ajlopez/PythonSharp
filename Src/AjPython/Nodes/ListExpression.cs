namespace AjPython.Nodes
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ListExpression : Expression
    {
        private List<Expression> expressions = new List<Expression>();

        public ListExpression()
        {
        }

        public List<Expression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }

        public void Add(Expression expression)
        {
            this.expressions.Add(expression);
        }

        public override object Evaluate(Environment environment)
        {
            IList list = new ArrayList();

            foreach (Expression expression in expressions)
            {
                list.Add(expression.Evaluate(environment));
            }

            return list;
        }
    }
}
