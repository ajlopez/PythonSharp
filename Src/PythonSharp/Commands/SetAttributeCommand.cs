namespace PythonSharp.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Expressions;
    using PythonSharp.Language;

    public class SetAttributeCommand : ICommand
    {
        private IExpression targetExpression;
        private string name;
        private IExpression expression;

        public SetAttributeCommand(IExpression targetExpression, string name, IExpression expression)
        {
            this.targetExpression = targetExpression;
            this.name = name;
            this.expression = expression;
        }

        public IExpression TargetExpression { get { return this.targetExpression; } }

        public string Name { get { return this.name; } }

        public IExpression Expression { get { return this.expression; } }

        public void Execute(IContext context)
        {
            IValues values = (IValues) this.targetExpression.Evaluate(context);
            values.SetValue(this.name, this.expression.Evaluate(context));
        }
    }
}
