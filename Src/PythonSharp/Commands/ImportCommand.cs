namespace PythonSharp.Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using PythonSharp.Compiler;
    using PythonSharp.Expressions;
    using PythonSharp.Language;
    using PythonSharp.Utilities;
    using PythonSharp.Exceptions;

    public class ImportCommand : ICommand
    {
        private string modname;
        private string[] names;

        public ImportCommand(string modname)
        {
            this.modname = modname;
            this.names = modname.Split('.');
        }

        public string ModuleName { get { return this.modname; } }

        public void Execute(IContext context)
        {
            Module module = null;
            string doc = null;

            if (TypeUtilities.IsNamespace(this.modname))
            {
                var types = TypeUtilities.GetTypesByNamespace(this.modname);

                module = new Module(context.GlobalContext);

                foreach (var type in types)
                    module.SetValue(type.Name, type);
            }
            else
            {
                string filename = ModuleUtilities.ModuleFileName(this.modname);

                if (filename == null)
                    throw new ImportError(string.Format("No module named {0}", this.modname));

                Parser parser = new Parser(new StreamReader(filename));
                ICommand command = parser.CompileCommandList();
                doc = CommandUtilities.GetDocString(command);

                module = new Module(context.GlobalContext);

                command.Execute(module);
            }

            IValues values = context;
            int nname = 0;

            foreach (var name in this.names)
            {
                string normname = name.Trim();

                if (nname == this.names.Length - 1)
                    values.SetValue(normname, module);
                else if (!values.HasValue(normname))
                {
                    var mod = new Module(context.GlobalContext);
                    values.SetValue(normname, mod);
                    values = mod;
                }
                else
                    values = (IValues)values.GetValue(normname);

                nname++;
            }

            module.SetValue("__doc__", doc);
        }
    }
}
