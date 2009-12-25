namespace AjPython.Expressions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DictionaryExpression : IExpression
    {
        private List<IExpression> keyExpressions = new List<IExpression>();
        private List<IExpression> valueExpressions = new List<IExpression>();

        public DictionaryExpression()
        {
        }

        public List<IExpression> KeyExpressions
        {
            get
            {
                return this.keyExpressions;
            }
        }

        public List<IExpression> ValueExpressions
        {
            get
            {
                return this.valueExpressions;
            }
        }

        public void Add(IExpression keyExpression, IExpression valueExpression)
        {
            this.keyExpressions.Add(keyExpression);
            this.valueExpressions.Add(valueExpression);
        }

        public object Evaluate(BindingEnvironment environment)
        {
            IDictionary dictionary = new Hashtable();

            int n = 0;

            foreach (IExpression keyExpression in this.keyExpressions)
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
