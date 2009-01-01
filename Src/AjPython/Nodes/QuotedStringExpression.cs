namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class QuotedStringExpression : Expression
    {
        private string value;

        public QuotedStringExpression(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get
            {
                return this.value;
            }
        }

        public override object Evaluate(Environment environment)
        {
            return this.value;
        }
    }
}
