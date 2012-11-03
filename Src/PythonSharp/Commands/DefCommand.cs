namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DefCommand : ICommand
    {
        private string name;
        private IList<string> argumentNames;
        private ICommand body;

        public DefCommand(string name, IList<string> argumentNames, ICommand body)
        {
            this.name = name;
            this.argumentNames = argumentNames;
            this.body = body;
        }

        public string Name { get { return this.name; } }

        public IList<string> ArgumentNames { get { return this.argumentNames; } }

        public ICommand Body { get { return this.body; } }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            throw new NotImplementedException();
        }
    }
}
