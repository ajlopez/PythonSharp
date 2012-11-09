namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Expressions;

    public class CompositeCommand : ICommand
    {
        private IList<ICommand> commands;

        public CompositeCommand()
        {
            this.commands = new List<ICommand>();
        }

        public CompositeCommand(IList<ICommand> commands)
        {
            this.commands = commands;
        }

        public ICollection<ICommand> Commands { get { return this.commands; } }

        public void AddCommand(ICommand command)
        {
            this.commands.Add(command);
        }

        public void Execute(BindingEnvironment environment)
        {
            foreach (ICommand command in this.commands)
            {
                command.Execute(environment);
                if (environment.HasReturnValue())
                    break;
            }
        }

        public string GetDocString()
        {
            if (this.commands == null || this.commands.Count == 0)
                return null;

            var first = this.commands[0];

            if (!(first is ExpressionCommand))
                return null;

            var exprcmd = (ExpressionCommand)first;

            if (!(exprcmd.Expression is ConstantExpression))
                return null;

            var consexpr = (ConstantExpression)exprcmd.Expression;

            if (consexpr.Value == null || !(consexpr.Value is string))
                return null;

            var str = (string)consexpr.Value;

            this.commands.RemoveAt(0);

            return str;
        }
    }
}
