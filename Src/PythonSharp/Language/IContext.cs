namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IContext : IValues
    {
        IContext GlobalContext { get; }
    }
}
