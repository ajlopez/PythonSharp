namespace PythonSharp.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using PythonSharp.Expressions;
    using PythonSharp.Language;

    public class TryCommand : ICommand
    {
        private ICommand command;
        private ICommand finallyCommand;

        public TryCommand(ICommand command)
        {
            this.command = command;
        }

        public ICommand Command { get { return this.command; } }

        public void SetFinally(ICommand finallyCommand)
        {
            this.finallyCommand = finallyCommand;
        }

        public void Execute(IContext context)
        {
            try
            {
                this.command.Execute(context);
            }
            finally
            {
                if (this.finallyCommand != null)
                    this.finallyCommand.Execute(context);
            }
        }
    }
}
