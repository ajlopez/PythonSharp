namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IObject : IValues
    {
        object Invoke(string name, IList<object> arguments);
    }
}
