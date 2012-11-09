namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;

    public class CallExpression : IExpression
    {
        private IExpression targetExpression;
        private IList<IExpression> argumentExpressions;
        private bool isobject;
        private bool hasnames;

        public CallExpression(IExpression targetExpression, IList<IExpression> argumentExpressions)
        {
            this.targetExpression = targetExpression;
            this.argumentExpressions = argumentExpressions;

            this.isobject = this.targetExpression is AttributeExpression;

            if (argumentExpressions != null)
            {
                IList<string> names = new List<string>();

                foreach (var argexpr in argumentExpressions)
                    if (argexpr is NamedArgumentExpression)
                    {
                        var namexpr = (NamedArgumentExpression)argexpr;
                        if (names.Contains(namexpr.Name))
                            throw new SyntaxError("keyword argument repeated");
                        names.Add(namexpr.Name);
                        this.hasnames = true;
                    }
                    else if (this.hasnames)
                        throw new SyntaxError("non-keyword arg after keyword arg");
            }
        }

        public IExpression TargetExpression { get { return this.targetExpression; } }

        public IList<IExpression> ArgumentExpressions { get { return this.argumentExpressions; } }

        public object Evaluate(BindingEnvironment environment)
        {
            IList<object> arguments = null;
            IDictionary<string, object> namedArguments = null;

            if (this.hasnames)
                namedArguments = new Dictionary<string, object>();

            if (this.argumentExpressions != null && this.argumentExpressions.Count > 0)
            {
                arguments = new List<object>();

                foreach (var argexpr in this.argumentExpressions)
                {
                    object value = argexpr.Evaluate(environment);

                    if (this.hasnames && argexpr is NamedArgumentExpression)
                        namedArguments[((NamedArgumentExpression)argexpr).Name] = value;
                    else
                        arguments.Add(argexpr.Evaluate(environment));
                }
            }

            if (this.isobject)
            {
                var attrexpr = (AttributeExpression)this.targetExpression;
                var obj = attrexpr.Expression.Evaluate(environment);

                // TODO when is not DynamicObject the target expression is evaluated twice
                if (obj is DynamicObject)
                {
                    var dynobj = (DynamicObject)obj;
                    return dynobj.InvokeMethod(attrexpr.Name, environment, arguments, namedArguments);
                }

                // TODO review, obj as self ONLY if the targetExpression is not a IType/DefinedClass
                arguments.Insert(0, obj);
            }

            IFunction function = (IFunction)this.targetExpression.Evaluate(environment);
            return function.Apply(environment, arguments, namedArguments);
        }
    }
}
