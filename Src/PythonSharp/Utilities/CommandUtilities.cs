namespace PythonSharp.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Commands;

    public static class CommandUtilities
    {
        public static string GetDocString(ICommand command)
        {
            var composite = command as CompositeCommand;

            if (composite == null)
                return null;

            return composite.GetDocString();
        }
    }
}
