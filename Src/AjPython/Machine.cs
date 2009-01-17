namespace AjPython
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Machine
    {
        private Environment environment = new Environment();
        private TextReader input = System.Console.In;
        private TextWriter output = System.Console.Out;

        public Environment Environment
        {
            get { return this.environment; }
        }

        public TextReader Input
        {
            get
            {
                return this.input;
            }

            set
            {
                this.input = value;
            }
        }

        public TextWriter Output
        {
            get
            {
                return this.output;
            }

            set
            {
                this.output = value;
            }
        }
    }
}

