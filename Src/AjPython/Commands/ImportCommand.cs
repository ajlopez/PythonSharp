namespace AjPython.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython.Expressions;
    using AjPython.Compiler;
    using AjPython.Utilities;
    using System.IO;

    public class ImportCommand : ICommand
    {
        private string modname;

        public ImportCommand(string modname)
        {
            this.modname = modname;
        }

        public string ModuleName { get { return this.modname; } }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            Parser parser = new Parser(new StreamReader(ModuleUtilities.ModuleFileName(this.modname)));
            ICommand command = parser.CompileCommandList();

            BindingEnvironment modenv = new BindingEnvironment();

            command.Execute(machine, modenv);

            environment.SetValue(this.modname, modenv);
        }
    }
}
