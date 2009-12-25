namespace AjPython.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StringExpression : IExpression
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

        public object Evaluate(BindingEnvironment environment)
        {
            return this.value;
        }
    }
}
