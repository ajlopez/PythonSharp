namespace AjPython.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BooleanExpression : IExpression
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

        public object Evaluate(BindingEnvironment env)
        {
            return this.value;
        }
    }
}
