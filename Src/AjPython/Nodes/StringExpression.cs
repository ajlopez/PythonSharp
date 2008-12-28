namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StringExpression : Expression
    {
        private string value;

        public StringExpression(string value)
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

        public override object Evaluate(Environment env)
        {
            return this.value;
        }
    }
}
