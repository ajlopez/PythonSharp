namespace AjPython.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AjPython.Language;

    public class BinaryOperatorExpression : BinaryExpression
    {
        private BinaryOperator @operator;

        public BinaryOperatorExpression(IExpression left, IExpression right, BinaryOperator oper)
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
                case BinaryOperator.Add:
                    return Numbers.Add(leftvalue, rightvalue);
                case BinaryOperator.Subtract:
                    return Numbers.Subtract(leftvalue, rightvalue);
                case BinaryOperator.Multiply:
                    return Numbers.Multiply(leftvalue, rightvalue);
                case BinaryOperator.Divide:
                    return Numbers.Divide(leftvalue, rightvalue);
                case BinaryOperator.Power:
                    return System.Convert.ToInt32(System.Math.Pow((int)leftvalue, (int)rightvalue));
            }

            throw new System.InvalidOperationException();
        }
    }
}
