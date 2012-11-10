﻿namespace PythonSharp.Expressions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class ListExpression : IExpression
    {
        private List<IExpression> expressions = new List<IExpression>();

        public ListExpression()
        {
        }

        public List<IExpression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }

        public void Add(IExpression expression)
        {
            this.expressions.Add(expression);
        }

        public object Evaluate(IContext context)
        {
            IList list = new ArrayList();

            foreach (IExpression expression in this.expressions)
                list.Add(expression.Evaluate(context));

            return list;
        }
    }
}
