namespace AjPython.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RealExpression : IExpression
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

        public object Evaluate(BindingEnvironment env)
        {
            return this.value;
        }
    }
}
