namespace PythonSharp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;
    using PythonSharp.Utilities;

    public class PrintFunction : IFunction
    {
        private Machine machine;

        public PrintFunction(Machine machine)
        {
            this.machine = machine;
        }

        public object Apply(IContext context, IList<object> arguments, IDictionary<string, object> namedArguments)
        {
            string separator = (namedArguments != null && namedArguments.ContainsKey("sep")) ? (string) namedArguments["sep"] : " ";
            string end = (namedArguments != null && namedArguments.ContainsKey("end")) ? (string)namedArguments["end"] : null;

            if (arguments != null)
            {
                int narg = 0;
                foreach (var argument in arguments)
                {
                    if (narg != 0)
                        this.machine.Output.Write(separator);
                    this.machine.Output.Write(ValueUtilities.AsPrintString(argument));
                    narg++;
                }
            }

            if (end != null)
                this.machine.Output.Write(end);
            else
                this.machine.Output.WriteLine();

            return null;
        }
    }
}
