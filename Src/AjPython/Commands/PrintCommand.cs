namespace AjPython.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython.Nodes;

    public class PrintCommand : Command
    {
        private Expression expression;

        public PrintCommand(Expression expression)
        {
            if (expression == null)
            {
                throw new System.ArgumentNullException("expression");
            }

            this.expression = expression;
        }

        public Expression Expression
        {
            get
            {
                return this.expression;
            }
        }

        public override void Execute(Machine machine)
        {
            machine.Output.WriteLine(this.expression.Evaluate(machine.Environment).ToString());
        }
    }
}
