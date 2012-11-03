namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Expressions;

    public class SetCommand : ICommand
    {
        private NameExpression variable;
        private IExpression expression;

        public SetCommand(NameExpression variable, IExpression expression)
        {
            this.variable = variable;
            this.expression = expression;
        }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            environment.SetValue(this.variable.Name, this.expression.Evaluate(environment));
        }
    }
}
