namespace AjPython.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CompositeCommand : ICommand
    {
        private IList<ICommand> commands;

        public CompositeCommand()
        {
            commands = new List<ICommand>();
        }

        public CompositeCommand(IList<ICommand> commands)
        {
            this.commands = commands;
        }

        public ICollection<ICommand> Commands { get { return this.commands; } }

        public void AddCommand(ICommand command)
        {
            commands.Add(command);
        }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            foreach (ICommand command in commands)
                command.Execute(machine, environment);
        }
    }
}
