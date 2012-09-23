namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Slice
    {
        int? begin;
        int? end;

        public Slice(int? begin, int? end)
        {
            this.begin = begin;
            this.end = end;
        }

        public int? Begin { get { return this.begin; } }

        public int? End { get { return this.end; } }
    }
}
