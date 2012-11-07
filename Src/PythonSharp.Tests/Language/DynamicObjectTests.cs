namespace PythonSharp.Tests.Language
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Language;

    [TestClass]
    public class DynamicObjectTests
    {
        [TestMethod]
        public void NewDynamicObject()
        {
            DefinedClass klass = new DefinedClass("Spam");
            DynamicObject dynobj = new DynamicObject(klass);

            Assert.IsNotNull(dynobj.Class);
            Assert.AreEqual(klass, dynobj.Class);
        }

        [TestMethod]
        public void GetMethodFromClass()
        {
            DefinedClass klass = new DefinedClass("Spam");
            IFunction function = new NativeMethod(DummyMethod);
            klass.SetMethod("foo", function);
            DynamicObject dynobj = new DynamicObject(klass);

            var result = dynobj.GetValue("foo");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IFunction));
            Assert.AreEqual(function, result);
        }

        [TestMethod]
        public void InvokeMethodDefinedInClass()
        {
            DefinedClass klass = new DefinedClass("Spam");
            IFunction function = new NativeMethod(DummyMethod);
            klass.SetMethod("foo", function);
            DynamicObject dynobj = new DynamicObject(klass);

            var result = dynobj.InvokeMethod("foo", null, null, null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void InvokeMethodThatReturnsSelf()
        {
            DefinedClass klass = new DefinedClass("Spam");
            IFunction function = new NativeMethod(SelfMethod);
            klass.SetMethod("foo", function);
            DynamicObject dynobj = new DynamicObject(klass);

            var result = dynobj.InvokeMethod("foo", null, null, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(dynobj, result);
        }

        [TestMethod]
        public void GetUndefinedAttributeAsNull()
        {
            DefinedClass klass = new DefinedClass("Spam");
            DynamicObject dynobj = new DynamicObject(klass);

            var result = dynobj.GetValue("foo");

            Assert.IsNull(result);
            Assert.IsFalse(dynobj.HasValue("foo"));
        }

        [TestMethod]
        public void SetGetValue()
        {
            DefinedClass klass = new DefinedClass("Spam");
            DynamicObject dynobj = new DynamicObject(klass);

            dynobj.SetValue("one", 1);
            var result = dynobj.GetValue("one");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
            Assert.IsTrue(dynobj.HasValue("one"));
        }

        private static object DummyMethod(IList<object> arguments)
        {
            return null;
        }

        private static object SelfMethod(IList<object> arguments)
        {
            return arguments[0];
        }
    }
}
