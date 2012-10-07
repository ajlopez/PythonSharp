namespace PythonSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Types
    {
        public static string GetTypeName(object value)
        {
            if (value == null)
                return "NoneType";

            if (value is int)
                return "int";

            return value.GetType().Name;
        }
    }
}
