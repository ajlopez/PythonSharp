namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ClassCommand : ICommand
    {
        private string name;
        private ICommand body;

        public ClassCommand(string name, ICommand body)
        {
            this.name = name;
            this.body = body;
        }

        public string Name { get { return this.name; } }

        public ICommand Body { get { return this.body; } }

        public void Execute(BindingEnvironment environment)
        {
            throw new NotImplementedException();
        }
    }
}
