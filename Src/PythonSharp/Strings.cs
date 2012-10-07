namespace PythonSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualBasic.CompilerServices;

    public static class Strings
    {
        public static object Multiply(object obj1, object obj2)
        {
            string text = (string)obj1;
            int repeat = (int)obj2;
            string result = string.Empty;

            for (int k = 0; k < repeat; k++)
                result += text;

            return result;
        }

        public static bool IsString(object obj)
        {
            return obj is string;
        }
    }
}
