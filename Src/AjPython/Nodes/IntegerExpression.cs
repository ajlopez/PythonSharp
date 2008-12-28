namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IntegerExpression : Expression
    {
        private int value;

        public IntegerExpression(int value)
        {
            this.value = value;
        }

        public int Value
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
