namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    public class ExceptCommand : ICommand
    {
        private IExpression expression;
        private ICommand command;

        public ExceptCommand(IExpression expression, ICommand command)
        {
            this.expression = expression;
            this.command = command;
        }

        public ICommand Command { get { return this.command; } }

        public IExpression Expression { get { return this.expression; } }

        public bool CatchException(IContext context, Exception exception)
        {
            if (this.expression == null)
                return true;

            var value = this.expression.Evaluate(context);

            if (!(value is Type) || !((Type)value == typeof(Exception) || ((Type)value).IsSubclassOf(typeof(Exception))))
                throw new TypeError("catching classes that do not inherit from BaseException is not allowed");

            var type = (Type)value;

            return exception.GetType() == type || exception.GetType().IsSubclassOf(type);
        }

        public void Execute(IContext context)
        {
            this.command.Execute(context);
        }
    }
}
