namespace PythonSharp.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class ValueUtilities
    {
        public static string AsString(object value)
        {
            if (value == null)
                return null;

            if (value is string)
            {
                string text = (string)value;
                
                if (text.IndexOf('\'') >= 0)
                {
                    if (text.IndexOf('"') >= 0)
                    {
                        text = text.Replace("'", "\\'");
                        return string.Format("'{0}'", text);
                    }

                    return string.Format("\"{0}\"", text);
                }

                return string.Format("'{0}'", text);
            }

            return value.ToString();
        }

        public static string AsPrintString(object value)
        {
            if (value == null)
                return "None";

            if (value is string)
                return (string)value;

            if (value is IList)
            {
                bool istuple = value is ReadOnlyCollection<object>;
                StringBuilder builder = new StringBuilder();

                if (istuple)
                    builder.Append("(");
                else
                    builder.Append("[");

                int nitems = 0;

                foreach (var item in (IList)value)
                {
                    if (nitems > 0)
                        builder.Append(", ");

                    builder.Append(AsString(item));

                    nitems++;
                }

                if (istuple)
                    builder.Append(")");
                else
                    builder.Append("]");

                return builder.ToString();
            }

            return value.ToString();
        }
    }
}
