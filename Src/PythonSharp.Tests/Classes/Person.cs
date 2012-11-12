namespace PythonSharp.Tests.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public delegate int MyEvent(string n);

    public class Person
    {
        public event MyEvent NameEvent;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string GetName()
        {
            if (this.NameEvent != null)
                this.NameEvent.Invoke(this.FirstName);

            return this.LastName + ", " + this.FirstName;
        }
    }
}
