namespace AjPython.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class BinaryExpression : Expression
    {
        private Expression left;
        private Expression right;

        public BinaryExpression(Expression left, Expression right)
        {
            if (left == null)
            {
                throw new System.ArgumentNullException("left");
            }

            if (right == null)
            {
                throw new System.ArgumentNullException("right");
            }
                
            this.left = left;
            this.right = right;
        }

        public Expression Left
        {
            get
            {
                return this.left;
            }
        }

        public Expression Right
        {
            get
            {
                return this.right;
            }
        }
    }
}
