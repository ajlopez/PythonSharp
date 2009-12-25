namespace AjPython.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IntegerExpression : IExpression
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

        public object Evaluate(BindingEnvironment env)
        {
            return this.value;
        }
    }
}
