namespace AjPython.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BinaryOperatorExpression : BinaryExpression
    {
        private Operator @operator;

        public BinaryOperatorExpression(IExpression left, IExpression right, Operator oper)
            : base(left, right)
        {
            this.@operator = oper;
        }

        public override object Evaluate(BindingEnvironment environment)
        {
            object leftvalue;
            object rightvalue;

            leftvalue = this.Left.Evaluate(environment);
            rightvalue = this.Right.Evaluate(environment);

            switch (this.@operator)
            {
                case Operator.Add:
                    return Numbers.Add(leftvalue, rightvalue);
                case Operator.Subtract:
                    return Numbers.Subtract(leftvalue, rightvalue);
                case Operator.Multiply:
                    return Numbers.Multiply(leftvalue, rightvalue);
                case Operator.Divide:
                    return Numbers.Divide(leftvalue, rightvalue);
                case Operator.Power:
                    return System.Convert.ToInt32(System.Math.Pow((int)leftvalue, (int)rightvalue));
            }

            throw new System.InvalidOperationException();
        }
    }
}
