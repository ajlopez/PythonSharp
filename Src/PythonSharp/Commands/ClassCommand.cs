namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;
    using PythonSharp.Utilities;

    public class ClassCommand : ICommand
    {
        private string name;
        private ICommand body;
        private string doc;

        public ClassCommand(string name, ICommand body)
        {
            this.name = name;
            this.body = body;
            this.doc = CommandUtilities.GetDocString(this.body);
        }

        public string Name { get { return this.name; } }

        public ICommand Body { get { return this.body; } }

        public void Execute(IContext context)
        {
            BindingEnvironment env = new BindingEnvironment(context);
            DefinedClass klass = new DefinedClass(this.name, context);
            this.body.Execute(klass);
            foreach (var name in env.GetNames())
            {
                var value = env.GetValue(name);
                var deffunc = value as DefinedFunction;

                if (deffunc != null)
                    klass.SetMethod(deffunc.Name, deffunc);
            }

            klass.SetValue("__doc__", doc);

            context.SetValue(this.name, klass);
        }
    }
}
