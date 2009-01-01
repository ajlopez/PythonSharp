namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BooleanExpression : Expression
    {
        private bool value;

        public BooleanExpression(bool value)
        {
            this.value = value;
        }

        public bool Value
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
