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
    using PythonSharp.Utilities.TypeUtilities;

    public class ImportCommand : ICommand
    {
        private string modname;

        public ImportCommand(string modname)
        {
            this.modname = modname;
        }

        public string ModuleName { get { return this.modname; } }

        public void Execute(IContext context)
        {
            Parser parser = new Parser(new StreamReader(ModuleUtilities.ModuleFileName(this.modname)));
            ICommand command = parser.CompileCommandList();
            string doc = CommandUtilities.GetDocString(command);

            Module module = new Module(context.GlobalContext);

            command.Execute(module);

            context.SetValue(this.modname, module);

            module.SetValue("__doc__", doc);
        }
    }
}
