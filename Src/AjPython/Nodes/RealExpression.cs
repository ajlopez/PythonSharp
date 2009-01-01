namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RealExpression : Expression
    {
        private double value;

        public RealExpression(double value)
        {
            this.value = value;
        }

        public double Value
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
