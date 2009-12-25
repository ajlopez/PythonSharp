namespace AjPython.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython.Expressions;

    public class PrintCommand : ICommand
    {
        private IExpression expression;

        public PrintCommand(IExpression expression)
        {
            if (expression == null)
                throw new System.ArgumentNullException("expression");

            this.expression = expression;
        }

        public IExpression Expression { get { return this.expression; } }

        public void Execute(Machine machine)
        {
            machine.Output.WriteLine(this.expression.Evaluate(machine.Environment).ToString());
        }
    }
}
