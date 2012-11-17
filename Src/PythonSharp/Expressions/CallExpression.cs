namespace PythonSharp.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;
    using PythonSharp.Utilities;

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

        public object Evaluate(IContext context)
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
                    object value = argexpr.Evaluate(context);

                    if (this.hasnames && argexpr is NamedArgumentExpression)
                        namedArguments[((NamedArgumentExpression)argexpr).Name] = value;
                    else
                        arguments.Add(argexpr.Evaluate(context));
                }
            }

            IFunction function = null;

            // TODO to skip AttributeExpression, or have a separated MethodCallExpression 
            if (this.isobject)
            {
                var attrexpr = (AttributeExpression)this.targetExpression;
                var obj = attrexpr.Expression.Evaluate(context);

                if (obj is DynamicObject)
                {
                    var dynobj = (DynamicObject)obj;
                    return dynobj.Invoke(attrexpr.Name, context, arguments, namedArguments);
                }

                function = this.GetFunction(obj, attrexpr.Name);

                if (function == null)
                {
                    IType type = Types.GetType(obj);

                    if (type == null)
                    {
                        IValues values = obj as IValues;

                        if (values != null && values.HasValue(attrexpr.Name))
                        {
                            object value = values.GetValue(attrexpr.Name);
                            function = value as IFunction;

                            if (function == null)
                            {
                                if (value is Type)
                                {
                                    return Activator.CreateInstance((Type)value, arguments == null ? null : arguments.ToArray());
                                }
                            }
                        }

                        if (function == null)
                        {
                            if (obj is Type)
                                return TypeUtilities.InvokeTypeMember((Type)obj, attrexpr.Name, arguments);

                            return ObjectUtilities.GetValue(obj, attrexpr.Name, arguments);
                        }
                    }

                    function = type.GetMethod(attrexpr.Name);
                    arguments.Insert(0, obj);
                }
            }
            else
            {
                var value = this.targetExpression.Evaluate(context);
                function = value as IFunction;

                if (function == null)
                {
                    Type type = (Type)value;
                    return Activator.CreateInstance(type, arguments == null ? null : arguments.ToArray());
                }
            }

            return function.Apply(context, arguments, namedArguments);
        }

        private IFunction GetFunction(object obj, string name)
        {
            IFunction function = null;

            if (obj is IType) 
            {
                function = ((IType)obj).GetMethod(name);
                if (function != null)
                    return function;
            }

            if (obj is IValues)
            {
                function = ((IValues)obj).GetValue(name) as IFunction;
                if (function != null)
                    return function;
            }

            return null;
        }
    }
}
