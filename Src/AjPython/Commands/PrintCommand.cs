namespace AjPython.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython.Expressions;

    public class PrintCommand : ICommand
    {
        private IList<IExpression> expressions;

        public PrintCommand(IList<IExpression> expressions)
        {
            this.expressions = expressions;
        }

        public ICollection<IExpression> Expressions { get { return this.expressions; } }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            if (this.expressions != null)
                foreach (IExpression expression in this.expressions)
                    machine.Output.Write(expression.Evaluate(environment).ToString());

            machine.Output.WriteLine();
        }
    }
}
