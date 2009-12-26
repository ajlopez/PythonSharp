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

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            foreach (ICommand command in this.commands)
                command.Execute(machine, environment);
        }
    }
}
