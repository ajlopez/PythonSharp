namespace PythonSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public static class Types
    {
        private static IType stringType = new StringType();

        public static string GetTypeName(object value)
        {
            if (value == null)
                return "NoneType";

            if (value is int)
                return "int";

            return value.GetType().Name;
        }

        public static IType GetType(object value)
        {
            DynamicObject dynobj = value as DynamicObject;

            if (dynobj != null)
                return dynobj.Class;

            return stringType;
        }
    }
}
