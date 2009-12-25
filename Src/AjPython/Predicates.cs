namespace AjPython
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Predicates
    {
        public static bool IsFalse(object obj)
        {
            if (obj == null)
                return true;

            if (obj.Equals(false))
                return true;

            if (Numbers.IsFixnum(obj) && obj.Equals(0))
                return true;

            if (Numbers.IsRealnum(obj) && obj.Equals(0.0))
                return true;

            if (obj is string && string.IsNullOrEmpty((string)obj))
                return true;

            return false;
        }
    }
}
