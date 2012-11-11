namespace PythonSharp.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;

    public class Range : IEnumerable
    {
        private int from;
        private int to;
        private int step;

        public Range(int to)
            : this(0, to, 1)
        {
        }

        public Range(int from, int to)
            : this(from, to, 1)
        {
        }

        public Range(int from, int to, int step)
        {
            if (step == 0)
                throw new ValueError("range() arg 3 must not be zero");

            this.from = from;
            this.to = to;
            this.step = step;
        }

        public int From { get { return this.from; } }

        public int To { get { return this.to; } }

        public int Step { get { return this.step; } }

        public IEnumerator GetEnumerator()
        {
            if (this.step < 0)
                for (int k = this.from; k > this.to; k += this.step)
                    yield return k;
            else
                for (int k = this.from; k < this.to; k += this.step)
                    yield return k;
        }

        public IList ToList()
        {
            var list = new List<object>();

            foreach (var item in this)
                list.Add(item);

            return list;
        }
    }
}
