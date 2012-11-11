namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;
    using PythonSharp.Tests.Classes;

    [TestClass]
    public class TypesTests
    {
        [TestMethod]
        public void TypeName()
        {
            Assert.AreEqual("NoneType", Types.GetTypeName(null));
            Assert.AreEqual("int", Types.GetTypeName(123));
            Assert.AreEqual("str", Types.GetTypeName("spam"));
            Assert.AreEqual("float", Types.GetTypeName(1.2));
            Assert.AreEqual("function", Types.GetTypeName(new DefinedFunction("spam",null,null)));
            Assert.AreEqual("list", Types.GetTypeName(new object[] { 1, 2 }));
            Assert.AreEqual("TypeError", Types.GetTypeName(new TypeError(string.Empty)));
        }

        [TestMethod]
        public void GetDynamicObjectType()
        {
            DefinedClass klass = new DefinedClass("Spam");
            DynamicObject foo = (DynamicObject)klass.Apply(null, null, null);

            var result = Types.GetType(foo);

            Assert.IsNotNull(result);
            Assert.AreEqual(klass, result);
        }

        [TestMethod]
        public void GetNativeObjectObjectTypeAsNull()
        {
            Person person = new Person();

            var result = Types.GetType(person);

            Assert.IsNull(result);
        }
    }
}
