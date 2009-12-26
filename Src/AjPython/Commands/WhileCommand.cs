namespace AjPython.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython.Expressions;

    public class WhileCommand : ICommand
    {
        private IExpression condition;
        private ICommand command;

        public WhileCommand(IExpression condition, ICommand command)
        {
            this.condition = condition;
            this.command = command;
        }

        public IExpression Condition { get { return this.condition; } }

        public ICommand Command { get { return this.command; } }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            while (!Predicates.IsFalse(this.condition.Evaluate(environment)))
                this.command.Execute(machine, environment);
        }
    }
}
