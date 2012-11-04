namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Commands;

    public class DefinedFunction : IFunction
    {
        private IList<Parameter> parameters;
        private ICommand body;

        public DefinedFunction(IList<Parameter> parameters, ICommand body)
        {
            this.parameters = parameters;
            this.body = body;
        }

        public ICollection<Parameter> Parameters { get { return this.parameters; } }

        public ICommand Body { get { return this.body; } }

        public object Apply(BindingEnvironment env, IList<object> arguments)
        {
            Machine machine = env.Machine;
            BindingEnvironment environment = new BindingEnvironment(env);

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
