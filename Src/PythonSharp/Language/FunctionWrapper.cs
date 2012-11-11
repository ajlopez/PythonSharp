namespace PythonSharp.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class FunctionWrapper
    {
        private IFunction function;
        private IContext context;

        public FunctionWrapper(IFunction function, IContext context)
        {
            this.function = function;
            this.context = context;
        }

        protected IFunction Function { get { return this.function; } }

        protected IContext Context { get { return this.context; } }
    }

    public class FunctionWrapper<TR, TD> : FunctionWrapper
    {
        public FunctionWrapper(IFunction function, IContext context)
            : base(function, context)
        {
        }

        public Delegate CreateFunctionDelegate()
        {
            return Delegate.CreateDelegate(typeof(TD), this, "DoFunction");
        }

        public Delegate CreateActionDelegate()
        {
            return Delegate.CreateDelegate(typeof(TD), this, "DoAction");
        }

        public TR DoFunction()
        {
            return (TR)this.Function.Apply(this.Context, null, null);
        }

        public void DoAction()
        {
            this.Function.Apply(this.Context, null, null);
        }
    }

    public class FunctionWrapper<T1, TR, TD> : FunctionWrapper
    {
        public FunctionWrapper(IFunction function, IContext context)
            : base(function, context)
        {
        }

        public Delegate CreateFunctionDelegate()
        {
            return Delegate.CreateDelegate(typeof(TD), this, "DoFunction");
        }

        public Delegate CreateActionDelegate()
        {
            return Delegate.CreateDelegate(typeof(TD), this, "DoAction");
        }

        public TR DoFunction(T1 t1)
        {
            return (TR)this.Function.Apply(this.Context, new object[] { t1 }, null);
        }

        public void DoAction(T1 t1)
        {
            this.Function.Apply(this.Context, new object[] { t1 }, null);
        }
    }
    
    public class FunctionWrapper<T1, T2, TR, TD> : FunctionWrapper
    {
        public FunctionWrapper(IFunction function, IContext context)
            : base(function, context)
        {
        }

        public Delegate CreateFunctionDelegate()
        {
            return Delegate.CreateDelegate(typeof(TD), this, "DoFunction");
        }

        public Delegate CreateActionDelegate()
        {
            return Delegate.CreateDelegate(typeof(TD), this, "DoAction");
        }

        public TR DoFunction(T1 t1, T2 t2)
        {
            return (TR)this.Function.Apply(this.Context, new object[] { t1, t2 }, null);
        }

        public void DoAction(T1 t1, T2 t2)
        {
            this.Function.Apply(this.Context, new object[] { t1, t2 }, null);
        }
    }
    
    public class FunctionWrapper<T1, T2, T3, TR, TD> : FunctionWrapper
    {
        public FunctionWrapper(IFunction function, IContext context)
            : base(function, context)
        {
        }

        public Delegate CreateFunctionDelegate()
        {
            return Delegate.CreateDelegate(typeof(TD), this, "DoFunction");
        }

        public Delegate CreateActionDelegate()
        {
            return Delegate.CreateDelegate(typeof(TD), this, "DoAction");
        }

        public TR DoFunction(T1 t1, T2 t2, T3 t3)
        {
            return (TR)this.Function.Apply(this.Context, new object[] { t1, t2, t3 }, null);
        }

        public void DoAction(T1 t1, T2 t2, T3 t3)
        {
            this.Function.Apply(this.Context, new object[] { t1, t2, t3 }, null);
        }
    }
}
