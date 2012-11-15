namespace PythonSharp.Tests.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;
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
            IFunction method = new NativeMethod(this.DummyMethod);

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
            IFunction method = new NativeMethod(this.DummyMethod);

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
            IFunction method = new NativeMethod(this.DummyMethod);

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
            IFunction method = new NativeMethod(this.DummyMethod);

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
        public void GetMethodFromSuperClass()
        {
            DefinedClass super = new DefinedClass("object");
            IFunction method = new NativeMethod(this.DummyMethod);
            super.SetValue("foo", method);
            DefinedClass klass = new DefinedClass("Spam", new IType[] { super });

            var result = klass.GetMethod("foo");
            Assert.IsNotNull(result);
            Assert.IsTrue(klass.HasMethod("foo"));
        }

        [TestMethod]
        public void GetValueFromSuperClass()
        {
            DefinedClass super = new DefinedClass("object");
            super.SetValue("foo", 1);
            DefinedClass klass = new DefinedClass("Spam", new IType[] { super });

            var result = klass.GetValue("foo");
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
            Assert.IsTrue(klass.HasValue("foo"));
        }

        [TestMethod]
        public void CreateInstance()
        {
            DefinedClass klass = new DefinedClass("Spam");
            var result = klass.Apply(null, null, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));
            Assert.AreEqual(klass, ((DynamicObject)result).Class);
        }

        [TestMethod]
        public void CreateInstanceWithConstructor()
        {
            DefinedClass klass = new DefinedClass("Spam");
            ICommand body = new SetAttributeCommand(new NameExpression("self"), "name", new NameExpression("name"));
            DefinedFunction constructor = new DefinedFunction("__init__", new Parameter[] { new Parameter("self", null, false), new Parameter("name", null, false) }, body);
            klass.SetMethod(constructor.Name, constructor);
            var result = klass.Apply(null, new object[] { "Adam" }, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DynamicObject));

            var dynobj = (DynamicObject)result;
            Assert.AreEqual(klass, dynobj.Class);
            Assert.IsTrue(dynobj.HasValue("name"));
            var name = dynobj.GetValue("name");
            Assert.IsNotNull(name);
            Assert.AreEqual("Adam", name);
        }

        private object DummyMethod(IList<object> arguments)
        {
            return null;
        }
    }
}
