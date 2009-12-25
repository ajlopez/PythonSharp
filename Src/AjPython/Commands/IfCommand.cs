namespace AjPython.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython.Expressions;

    public class IfCommand : ICommand
    {
        private IExpression condition;
        private ICommand thencmd;

        public IfCommand(IExpression condition, ICommand thencmd)
        {
            this.condition = condition;
            this.thencmd = thencmd;
        }

        public IExpression Condition { get { return this.condition; } }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            bool isfalse = Predicates.IsFalse(this.condition.Evaluate(environment));

            if (!isfalse)
                this.thencmd.Execute(machine, environment);
        }
    }
}
