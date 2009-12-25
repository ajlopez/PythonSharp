namespace AjPython.Utilities
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
            FileInfo fi = new FileInfo(name + ".py");

            return fi.FullName;
        }
    }
}
