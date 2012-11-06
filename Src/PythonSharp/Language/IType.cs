namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IType
    {
        IFunction GetMethod(string name);
    }
}
