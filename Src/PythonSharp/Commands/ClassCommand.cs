namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

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
            BindingEnvironment env = new BindingEnvironment(environment);
            this.body.Execute(env);
            DefinedClass klass = new DefinedClass(this.name);
            foreach (var name in env.GetNames())
            {
                var value = env.GetValue(name);
                var deffunc = value as DefinedFunction;

                if (deffunc != null)
                    klass.SetMethod(deffunc.Name, deffunc);
            }

            environment.SetValue(this.name, klass);
        }
    }
}
