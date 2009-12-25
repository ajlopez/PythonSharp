namespace AjPython.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NameExpression : IExpression
    {
        private string name;

        public NameExpression(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public object Evaluate(BindingEnvironment environment)
        {
            return environment.GetValue(this.name);
        }
    }
}
