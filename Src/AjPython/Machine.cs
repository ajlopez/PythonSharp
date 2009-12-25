namespace AjPython
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Machine
    {
        private BindingEnvironment environment = new BindingEnvironment();
        private TextReader input = System.Console.In;
        private TextWriter output = System.Console.Out;

        public BindingEnvironment Environment
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

