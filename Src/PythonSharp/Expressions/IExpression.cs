namespace PythonSharp.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public interface IExpression
    {
        object Evaluate(IContext context);
    }
}
