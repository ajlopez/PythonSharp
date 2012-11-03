namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Expressions;

    public class SetCommand : ICommand
    {
        private string target;
        private IExpression expression;

        public SetCommand(string target, IExpression expression)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (expression == null)
                throw new ArgumentNullException("expression");

            this.target = target;
            this.expression = expression;
        }

        public string Target { get { return this.target; } }

        public IExpression Expression { get { return this.expression; } }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            environment.SetValue(this.target, this.expression.Evaluate(environment));
        }
    }
}
