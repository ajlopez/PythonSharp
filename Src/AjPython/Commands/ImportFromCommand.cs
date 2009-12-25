namespace AjPython.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython.Expressions;
    using AjPython.Compiler;
    using AjPython.Utilities;
    using System.IO;

    public class ImportFromCommand : ICommand
    {
        private string modname;
        private IList<string> names;

        public ImportFromCommand(string name, IList<string> names)
        {
            this.modname = name;
            this.names = names;
        }

        public string ModuleName { get { return this.modname; } }

        public ICollection<string> Names { get { return this.names; } }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            Parser parser = new Parser(new StreamReader(ModuleUtilities.ModuleFileName(this.modname)));
            ICommand command = parser.CompileCommandList();

            BindingEnvironment modenv = new BindingEnvironment();

            command.Execute(machine, modenv);

            foreach (string name in this.names)
                environment.SetValue(name, modenv.GetValue(name));
        }
    }
}
