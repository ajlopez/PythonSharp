namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class Expression
    {
        public abstract object Evaluate(Environment environment);
    }
}
