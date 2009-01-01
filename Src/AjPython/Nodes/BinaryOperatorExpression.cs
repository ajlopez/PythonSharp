namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BinaryOperatorExpression : BinaryExpression
    {
        private Operator @operator;

        public BinaryOperatorExpression(Expression left, Expression right, Operator oper)
            : base(left, right)
        {
            this.@operator = oper;
        }

        public override object Evaluate(Environment environment)
        {
            object leftvalue;
            object rightvalue;

            leftvalue = this.Left.Evaluate(environment);
            rightvalue = this.Right.Evaluate(environment);

            switch (this.@operator)
            {
                case Operator.Add:
                    return (int)leftvalue + (int)rightvalue;
                case Operator.Subtract:
                    return (int)leftvalue - (int)rightvalue;
                case Operator.Multiply:
                    return (int)leftvalue * (int)rightvalue;
                case Operator.Divide:
                    return (int)leftvalue / (int)rightvalue;
                case Operator.Power:
                    return System.Convert.ToInt32(System.Math.Pow((int)leftvalue, (int)rightvalue));
            }

            throw new System.InvalidOperationException();
        }
    }
}
