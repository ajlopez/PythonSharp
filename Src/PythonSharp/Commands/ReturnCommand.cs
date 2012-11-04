namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Expressions;

    public class ReturnCommand : ICommand
    {
        private IExpression expression;

        public ReturnCommand(IExpression expression)
        {
            this.expression = expression;
        }

        public void Execute(BindingEnvironment environment)
        {
            if (this.expression == null)
                environment.SetReturnValue(null);
            else
                environment.SetReturnValue(this.expression.Evaluate(environment));
        }
    }
}
