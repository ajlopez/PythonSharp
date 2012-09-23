namespace PythonSharp.Expressions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Language;

    public class SlicedExpression : IExpression
    {
        private IExpression targetExpression;
        private SliceExpression sliceExpression;

        public SlicedExpression(IExpression targetExpression, SliceExpression sliceExpression)
        {
            this.targetExpression = targetExpression;
            this.sliceExpression = sliceExpression;
        }

        public IExpression TargetExpression { get { return this.targetExpression; } }

        public IExpression IndexExpression { get { return this.sliceExpression; } }

        public object Evaluate(BindingEnvironment environment)
        {
            object target = this.targetExpression.Evaluate(environment);
            Slice slice = (Slice)this.sliceExpression.Evaluate(environment);
            int begin = slice.Begin ?? 0;
            int end;

            if (target is string)
            {
                string text = (string)target;
                end = slice.End ?? text.Length;
                if (end > text.Length)
                    end = text.Length;
                return text.Substring(begin, end - begin);
            }

            IList list = (IList)target;
            end = slice.End ?? list.Count;
            if (end > list.Count)
                end = list.Count;

            IList result = new List<object>();

            for (int k = begin; k < end && k < list.Count; k++)
                result.Add(list[k]);

            return result;
        }
    }
}
