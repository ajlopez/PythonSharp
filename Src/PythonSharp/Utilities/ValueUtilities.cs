namespace PythonSharp.Utilities
{
    using System;
    using System.Collections.Generic;
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
    }
}
