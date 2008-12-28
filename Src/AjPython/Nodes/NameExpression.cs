namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class NameExpression : Expression
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

        public override object Evaluate(Environment env)
        {
            return env.GetValue(this.name);
        }
    }
}
