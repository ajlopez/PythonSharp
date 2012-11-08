namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;

    [TestClass]
    public class TypesTests
    {
        [TestMethod]
        public void TypeName()
        {
            Assert.AreEqual("NoneType", Types.GetTypeName(null));
            Assert.AreEqual("int", Types.GetTypeName(1));
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
    }
}
