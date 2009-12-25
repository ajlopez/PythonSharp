namespace AjPython.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjPython.Expressions;

    public class SimpleAssignmentCommand : ICommand
    {
        private string name;
        private IExpression expression;

        public SimpleAssignmentCommand(string name, IExpression expression)
        {
            if (name == null)
                throw new System.ArgumentNullException("name");

            if (expression == null)
                throw new System.ArgumentNullException("expression");

            this.name = name;
            this.expression = expression;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public IExpression Expression { get { return this.expression; } }

        public void Execute(Machine machine, BindingEnvironment environment)
        {
            environment.SetValue(this.name, this.expression.Evaluate(environment));
        }
    }
}
