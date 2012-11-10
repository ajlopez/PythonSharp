namespace PythonSharp.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using PythonSharp.Expressions;
    using PythonSharp.Language;

    public class ExpressionCommand : ICommand
    {
        private IExpression expression;

        public ExpressionCommand(IExpression expression)
        {
            this.expression = expression;
        }

        public IExpression Expression { get { return this.expression; } }

        public void Execute(IContext context)
        {
            this.expression.Evaluate(context);
        }
    }
}
