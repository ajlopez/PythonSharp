namespace PythonSharp.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class ModuleUtilities
    {
        public static string ModuleFileName(string name)
        {
            string dirname = name.Replace('.', '/');
            string filename = dirname + ".py";
            string initfilename = dirname + "/__init__.py";

            string fullfilename = Path.Combine(".", filename);
            string fullinitfilename = Path.Combine(".", initfilename);

            if (File.Exists(fullfilename))
                return (new FileInfo(fullfilename)).FullName;

            if (File.Exists(fullinitfilename))
                return (new FileInfo(fullinitfilename)).FullName;

            return null;
        }
    }
}
