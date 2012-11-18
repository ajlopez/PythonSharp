namespace PythonSharp.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using PythonSharp.Expressions;
    using PythonSharp.Language;

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

        public void Execute(IContext context)
        {
            BindingEnvironment environment = context as BindingEnvironment;

            while (!Predicates.IsFalse(this.condition.Evaluate(context)))
            {
                this.command.Execute(context);

                if (environment != null)
                {
                    if (environment.HasReturnValue())
                        return;

                    if (environment.WasBreak)
                    {
                        environment.WasBreak = false;
                        break;
                    }

                    if (environment.WasContinue)
                        environment.WasContinue = false;
                }
            }
        }
    }
}
