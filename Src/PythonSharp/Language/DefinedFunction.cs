namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Commands;
    using PythonSharp.Exceptions;

    public class DefinedFunction : IFunction
    {
        private string name;
        private IList<Parameter> parameters;
        private int nparameters;
        private ICommand body;

        public DefinedFunction(string name, IList<Parameter> parameters, ICommand body)
        {
            this.name = name;
            this.parameters = parameters;
            this.body = body;

            if (parameters != null)
                this.nparameters = parameters.Count;
        }

        public string Name { get { return this.name; } }

        public ICollection<Parameter> Parameters { get { return this.parameters; } }

        public ICommand Body { get { return this.body; } }

        public object Apply(BindingEnvironment env, IList<object> arguments)
        {
            Machine machine = env.Machine;
            BindingEnvironment environment = new BindingEnvironment(env);

            int nargs = 0;

            if (arguments != null)
                nargs = arguments.Count;

            if (nargs != this.nparameters)
                throw new TypeError(string.Format("{0}() takes exactly {1} positional argument{2} ({3} given)", this.name, this.nparameters, this.nparameters == 1 ? "" : "s", nargs));

            if (this.parameters != null)
                for (int k = 0; k < this.parameters.Count; k++)
                    if (arguments != null && arguments.Count > k)
                        environment.SetValue(this.parameters[k].Name, arguments[k]);
                    else
                        environment.SetValue(this.parameters[k].Name, null);

            this.body.Execute(environment);

            if (environment.HasReturnValue())
                return environment.GetReturnValue();

            return null;
        }
    }
}
