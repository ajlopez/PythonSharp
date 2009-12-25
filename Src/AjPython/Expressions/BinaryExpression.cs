namespace AjPython.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class BinaryExpression : IExpression
    {
        private IExpression left;
        private IExpression right;

        protected BinaryExpression(IExpression left, IExpression right)
        {
            if (left == null)
                throw new System.ArgumentNullException("left");

            if (right == null)
                throw new System.ArgumentNullException("right");
                
            this.left = left;
            this.right = right;
        }

        public IExpression Left { get { return this.left; } }

        public IExpression Right { get { return this.right; } }

        public abstract object Evaluate(BindingEnvironment environment);
    }
}
