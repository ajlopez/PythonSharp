namespace PythonSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public static class Types
    {
        private static IType stringType = new StringType("str");

        public static string GetTypeName(object value)
        {
            if (value == null)
                return "NoneType";

            if (value is int)
                return "int";

            if (value is double)
                return "float";

            if (value is string)
                return "str";

            if (value is IFunction)
                return "function";

            if (value is IList)
                return "list";

            return value.GetType().Name;
        }

        public static IType GetType(object value)
        {
            if (value is string)
                return stringType;

            DynamicObject dynobj = value as DynamicObject;

            if (dynobj != null)
                return dynobj.Class;

            return null;
        }
    }
}
