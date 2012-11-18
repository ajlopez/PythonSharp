namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    public class BreakCommand : ICommand
    {
        public void Execute(IContext context)
        {
            BindingEnvironment environment = (BindingEnvironment)context;
            environment.WasBreak = true;
        }
    }
}
