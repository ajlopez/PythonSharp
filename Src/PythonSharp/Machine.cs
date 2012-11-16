namespace PythonSharp
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using PythonSharp.Functions;

    public class Machine
    {
        private BindingEnvironment environment;
        private TextReader input = System.Console.In;
        private TextWriter output = System.Console.Out;

        public Machine()
        {
            this.environment = new BindingEnvironment();
            this.environment.SetValue("len", new LenFunction());
            this.environment.SetValue("print", new PrintFunction(this));
            this.environment.SetValue("range", new RangeFunction());
            this.environment.SetValue("eval", new EvalFunction());
            this.environment.SetValue("exec", new ExecFunction());
            this.environment.SetValue("dir", new DirFunction());
            this.environment.SetValue("exit", new ExitFunction());
            this.environment.SetValue("id", new IdFunction());
            this.environment.SetValue("locals", new ContextFunction("locals", false));
            this.environment.SetValue("globals", new ContextFunction("globals", true));
            this.environment.SetValue("__machine__", this);
        }

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
