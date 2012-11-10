namespace PythonSharp.Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using PythonSharp.Compiler;
    using PythonSharp.Expressions;
    using PythonSharp.Utilities;
    using PythonSharp.Language;

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

        public void Execute(IContext context)
        {
            Parser parser = new Parser(new StreamReader(ModuleUtilities.ModuleFileName(this.modname)));
            ICommand command = parser.CompileCommandList();

            BindingEnvironment modenv = new BindingEnvironment(context.GlobalContext);

            command.Execute(modenv);

            foreach (string name in this.names)
                context.SetValue(name, modenv.GetValue(name));
        }
    }
}
