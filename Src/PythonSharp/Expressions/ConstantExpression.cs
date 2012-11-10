namespace PythonSharp.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class ConstantExpression : IExpression
    {
        private object value;

        public ConstantExpression(object value)
        {
            this.value = value;
        }

        public object Value { get { return this.value; } }

        public object Evaluate(IContext context)
        {
            return this.value;
        }
    }
}
