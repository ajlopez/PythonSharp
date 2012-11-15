namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;
    using PythonSharp.Utilities;
using PythonSharp.Expressions;

    public class ClassCommand : ICommand
    {
        private string name;
        private ICommand body;
        private IList<IExpression> baseExpressions;
        private string doc;

        public ClassCommand(string name, ICommand body)
            : this(name, null, body)
        {
        }

        public ClassCommand(string name, IList<IExpression> baseExpressions, ICommand body)
        {
            this.name = name;
            this.body = body;
            this.baseExpressions = baseExpressions;
            this.doc = CommandUtilities.GetDocString(this.body);
        }

        public string Name { get { return this.name; } }

        public ICommand Body { get { return this.body; } }

        public IList<IExpression> BaseExpressions { get { return this.baseExpressions; } }

        public void Execute(IContext context)
        {
            BindingEnvironment env = new BindingEnvironment(context);
            IList<IType> bases = null;

            if (this.baseExpressions != null && this.baseExpressions.Count > 0)
            {
                bases = new List<IType>();

                foreach (var expr in this.baseExpressions)
                    bases.Add((IType)expr.Evaluate(context));
            }

            DefinedClass klass = new DefinedClass(this.name, bases, context);
            this.body.Execute(klass);
            foreach (var name in env.GetNames())
            {
                var value = env.GetValue(name);
                var deffunc = value as DefinedFunction;

                if (deffunc != null)
                    klass.SetMethod(deffunc.Name, deffunc);
            }

            klass.SetValue("__doc__", this.doc);

            context.SetValue(this.name, klass);
        }
    }
}
