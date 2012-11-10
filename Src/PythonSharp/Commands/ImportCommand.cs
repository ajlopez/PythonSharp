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

            BindingEnvironment modenv = new BindingEnvironment(context.GlobalContext);

            command.Execute(modenv);

            context.SetValue(this.modname, modenv);

            modenv.SetValue("__doc__", doc);
        }
    }
}
