namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    // TODO add IValues, see StringType, then review DefinedClass iterations over bases
    public interface IType
    {
        IFunction GetMethod(string name);

        bool HasMethod(string name);
    }
}
