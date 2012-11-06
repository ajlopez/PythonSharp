namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public delegate object MethodDelegate(IList<object> arguments);

    public interface IMethod
    {
        object Apply(IList<object> arguments);
    }
}
