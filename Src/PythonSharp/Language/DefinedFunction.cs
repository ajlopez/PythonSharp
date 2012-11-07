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
        private int nminparameters;
        private int nmaxparameters;
        private int nparameters;
        private bool hasdefault;
        private bool haslist;
        private ICommand body;

        public DefinedFunction(string name, IList<Parameter> parameters, ICommand body)
        {
            this.name = name;
            this.parameters = parameters;
            this.body = body;

            if (parameters != null)
            {
                this.nparameters = parameters.Count;
                this.nmaxparameters = parameters.Count;
                foreach (var parameter in parameters)
                {
                    if (parameter.DefaultValue != null)
                        this.hasdefault = true;
                    
                    if (parameter.IsList)
                    {
                        this.haslist = true;
                        this.nmaxparameters = Int32.MaxValue;
                    }

                    if (!this.hasdefault && !this.haslist)
                        this.nminparameters++;
                }
            }
        }

        public string Name { get { return this.name; } }

        public ICollection<Parameter> Parameters { get { return this.parameters; } }

        public ICommand Body { get { return this.body; } }

        public object Apply(BindingEnvironment env, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            Machine machine = env.Machine;
            BindingEnvironment environment = new BindingEnvironment(env);

            int nargs = 0;

            if (arguments != null)
                nargs = arguments.Count;

            if (nargs < this.nminparameters || nargs > this.nmaxparameters)
                throw new TypeError(string.Format("{0}() takes {4} {1} positional argument{2} ({3} given)", this.name, this.nminparameters, this.nminparameters == 1 ? string.Empty : "s", nargs, this.hasdefault ? "at least" : "exactly"));

            if (namedArguments != null)
                foreach (var namarg in namedArguments)
                    environment.SetValue(namarg.Key, namarg.Value);

            if (this.parameters != null)
            {
                int k;

                for (k = 0; k < this.parameters.Count; k++)
                    if (arguments != null && arguments.Count > k)
                    {
                        if (namedArguments != null && namedArguments.ContainsKey(this.parameters[k].Name))
                            throw new TypeError(string.Format("{0}() got multiple values for keyword argument '{1}'", this.name, this.parameters[k].Name));
                        if (this.parameters[k].IsList)
                            environment.SetValue(this.parameters[k].Name, GetSublist(arguments, k));
                        else
                            environment.SetValue(this.parameters[k].Name, arguments[k]);
                    }
                    else if (this.parameters[k].IsList)
                    {
                        if (this.parameters[k].DefaultValue == null)
                            environment.SetValue(this.parameters[k].Name, new List<object>());
                        else
                            environment.SetValue(this.parameters[k].Name, this.parameters[k].DefaultValue);

                        break;
                    }
                    else if (namedArguments == null || !namedArguments.ContainsKey(this.parameters[k].Name))
                        environment.SetValue(this.parameters[k].Name, this.parameters[k].DefaultValue);
            }

            this.body.Execute(environment);

            if (environment.HasReturnValue())
                return environment.GetReturnValue();

            return null;
        }

        private static IList<object> GetSublist(IList<object> list, int from)
        {
            return list.Skip(from).ToList();
        }
    }
}
