namespace PythonSharp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class PrintFunction : IFunction
    {
        public object Apply(IList<object> arguments)
        {
            throw new NotImplementedException();
        }

        public object Apply(Machine machine, BindingEnvironment environment, IList<object> arguments)
        {
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
