namespace PythonSharp.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Language;

    [TestClass]
    public class DefinedClassTests
    {
        [TestMethod]
        public void DefineClassWithName()
        {
            DefinedClass klass = new DefinedClass("Spam");

            Assert.AreEqual("Spam", klass.Name);
        }

        [TestMethod]
        public void GetUndefinedAttributeAsNull()
        {
            DefinedClass klass = new DefinedClass("Spam");

            Assert.IsNull(klass.GetValue("foo"));
            Assert.IsFalse(klass.HasValue("foo"));
        }

        [TestMethod]
        public void SetGetAttribute()
        {
            DefinedClass klass = new DefinedClass("Spam");
            klass.SetValue("one", 1);

            var result = klass.GetValue("one");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
            Assert.IsTrue(klass.HasValue("one"));
        }

        [TestMethod]
        public void SetGetMethod()
        {
            DefinedClass klass = new DefinedClass("Spam");
            IFunction method = new NativeMethod(DummyMethod);

            klass.SetMethod("foo", method);

            var result = klass.GetMethod("foo");
            Assert.IsNotNull(result);
            Assert.AreEqual(method, result);
            Assert.IsTrue(klass.HasMethod("foo"));
            Assert.IsTrue(klass.HasValue("foo"));
        }

        [TestMethod]
        public void ReplaceMethodWithAttribute()
        {
            DefinedClass klass = new DefinedClass("Spam");
            IFunction method = new NativeMethod(DummyMethod);

            klass.SetMethod("foo", method);
            klass.SetValue("foo", 1);

            var result = klass.GetMethod("foo");
            Assert.IsNull(result);
            Assert.IsFalse(klass.HasMethod("foo"));
            Assert.AreEqual(1, klass.GetValue("foo"));
            Assert.IsTrue(klass.HasValue("foo"));
        }

        [TestMethod]
        public void DefineMethodUsingSetValue()
        {
            DefinedClass klass = new DefinedClass("Spam");
            IFunction method = new NativeMethod(DummyMethod);

            klass.SetValue("foo", method);

            var result = klass.GetMethod("foo");
            Assert.IsNotNull(result);
            Assert.AreEqual(method, result);
            Assert.IsTrue(klass.HasMethod("foo"));
            Assert.IsTrue(klass.HasValue("foo"));
        }

        [TestMethod]
        public void RedefineAttributeAsMethodUsingSetValue()
        {
            DefinedClass klass = new DefinedClass("Spam");
            IFunction method = new NativeMethod(DummyMethod);

            klass.SetValue("foo", 1);
            klass.SetValue("foo", method);

            var result = klass.GetMethod("foo");
            Assert.IsNotNull(result);
            Assert.AreEqual(method, result);
            Assert.IsTrue(klass.HasMethod("foo"));
            Assert.IsTrue(klass.HasValue("foo"));
        }

        [TestMethod]
        public void GetUndefinedMethodAsNull()
        {
            DefinedClass klass = new DefinedClass("Spam");

            var result = klass.GetMethod("foo");
            Assert.IsNull(result);
            Assert.IsFalse(klass.HasMethod("foo"));
        }

        [TestMethod]
        public void CreateInstance()
        {
            DefinedClass klass = new DefinedClass("Spam");
            var result = klass.Apply(null, null, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));
            Assert.AreEqual(((DynamicObject)result).Class, klass);
        }

        private object DummyMethod(IList<object> arguments)
        {
            return null;
        }
    }
}
