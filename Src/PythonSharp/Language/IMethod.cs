namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public delegate object MethodDelegate(object target, IList<object> arguments);

    public interface IMethod
    {
        object Apply(object target, IList<object> arguments);
    }
}
