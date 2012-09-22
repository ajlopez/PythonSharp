namespace PythonSharp.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using PythonSharp.Expressions;

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
            {
                int nexpr = 0;
                foreach (IExpression expression in this.expressions)
                {
                    if (nexpr != 0)
                        machine.Output.Write(' ');
                    machine.Output.Write(expression.Evaluate(environment).ToString());
                    nexpr++;
                }
            }

            machine.Output.WriteLine();
        }
    }
}
