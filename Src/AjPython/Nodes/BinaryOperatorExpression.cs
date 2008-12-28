namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BinaryOperatorExpression : BinaryExpression
    {
        private Operator oper;

        public BinaryOperatorExpression(Expression left, Expression right, Operator oper)
            : base(left, right)
        {
            this.oper = oper;
        }

        public override object Evaluate(Environment environment)
        {
            object leftvalue;
            object rightvalue;

            leftvalue = this.Left.Evaluate(environment);
            rightvalue = this.Right.Evaluate(environment);

            switch (this.oper)
            {
                case Operator.Add:
                    return (int)leftvalue + (int)rightvalue;
                case Operator.Substract:
                    return (int)leftvalue - (int)rightvalue;
                case Operator.Multiply:
                    return (int)leftvalue * (int)rightvalue;
                case Operator.Divide:
                    return (int)leftvalue / (int)rightvalue;
            }

            throw new System.InvalidOperationException();
        }
    }
}
