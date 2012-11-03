namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Commands;

    public class DefinedFunction : IFunction
    {
        private IList<string> argumentNames;
        private ICommand body;

        public DefinedFunction(IList<string> argumentNames, ICommand body)
        {
            this.argumentNames = argumentNames;
            this.body = body;
        }

        public ICollection<string> ArgumentNames { get { return this.argumentNames; } }

        public ICommand Body { get { return this.body; } }

        public object Apply(IList<object> arguments)
        {
            return this.Apply(null, null, arguments);
        }

        public object Apply(Machine machine, BindingEnvironment env, IList<object> arguments)
        {
            BindingEnvironment environment = new BindingEnvironment(env);

            if (this.argumentNames != null)
                for (int k = 0; k < this.argumentNames.Count; k++)
                    if (arguments != null && arguments.Count > k)
                        environment.SetValue(this.argumentNames[k], arguments[k]);
                    else
                        environment.SetValue(this.argumentNames[k], null);

            this.body.Execute(machine, environment);

            return null;
        }
    }
}
