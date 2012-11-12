namespace PythonSharp.Tests.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Calculator
    {
        public event Action<int, int> MyEvent;

        public static int Value { get { return 3; } }

        public int Add(int x, int y)
        {
            if (this.MyEvent != null)
                this.MyEvent(x, y);

            return x + y;
        }

        public int Subtract(int x, int y)
        {
            if (this.MyEvent != null)
                this.MyEvent(x, y);

            return x - y;
        }
    }
}
