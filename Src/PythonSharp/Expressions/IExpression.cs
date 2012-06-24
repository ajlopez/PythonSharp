namespace PythonSharp.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IExpression
    {
        object Evaluate(BindingEnvironment environment);
    }
}
