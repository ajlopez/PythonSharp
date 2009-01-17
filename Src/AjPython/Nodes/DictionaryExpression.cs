namespace AjPython.Nodes
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DictionaryExpression : Expression
    {
        private List<Expression> keyExpressions = new List<Expression>();
        private List<Expression> valueExpressions = new List<Expression>();

        public DictionaryExpression()
        {
        }

        public List<Expression> KeyExpressions
        {
            get
            {
                return this.keyExpressions;
            }
        }

        public List<Expression> ValueExpressions
        {
            get
            {
                return this.valueExpressions;
            }
        }

        public void Add(Expression keyExpression, Expression valueExpression)
        {
            this.keyExpressions.Add(keyExpression);
            this.valueExpressions.Add(valueExpression);
        }

        public override object Evaluate(Environment environment)
        {
            IDictionary dictionary = new Hashtable();

            int n = 0;

            foreach (Expression keyExpression in this.keyExpressions)
            {
                object key = keyExpression.Evaluate(environment);
                object value = this.valueExpressions[n].Evaluate(environment);
                dictionary[key] = value;
                n++;
            }

            return dictionary;
        }
    }
}
