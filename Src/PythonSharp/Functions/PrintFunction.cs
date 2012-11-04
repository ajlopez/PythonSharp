namespace PythonSharp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class PrintFunction : IFunction
    {
        public object Apply(BindingEnvironment environment, IList<object> arguments)
        {
            Machine machine = environment.Machine;

            if (arguments != null)
            {
                int narg = 0;
                foreach (var argument in arguments)
                {
                    if (narg != 0)
                        machine.Output.Write(' ');
                    machine.Output.Write(argument.ToString());
                    narg++;
                }
            }

            machine.Output.WriteLine();

            return null;
        }
    }
}
