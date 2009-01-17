namespace AjPython.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython.Nodes;

    public class SimpleAssignmentCommand : Command
    {
        private string name;
        private Expression expression;

        public SimpleAssignmentCommand(string name, Expression expression)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("name");
            }

            if (expression == null)
            {
                throw new System.ArgumentNullException("expression");
            }

            this.name = name;
            this.expression = expression;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
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
            machine.Environment.SetValue(this.name, this.expression.Evaluate(machine.Environment));
        }
    }
}
