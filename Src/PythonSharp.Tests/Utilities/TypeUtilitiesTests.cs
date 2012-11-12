namespace PythonSharp.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Compiler;
    using PythonSharp.Exceptions;
    using PythonSharp.Utilities;

    [TestClass]
    public class TypeUtilitiesTests
    {
        [TestMethod]
        public void GetTypeByName()
        {
            Type type = TypeUtilities.GetType("System.Int32");

            Assert.IsNotNull(type);
            Assert.AreEqual(type, typeof(int));
        }

        [TestMethod]
        public void GetTypeStoredInEnvironment()
        {
            BindingEnvironment environment = new BindingEnvironment();

            environment.SetValue("int", typeof(int));

            Type type = TypeUtilities.GetType(environment, "int");

            Assert.IsNotNull(type);
            Assert.AreEqual(type, typeof(int));
        }

        [TestMethod]
        public void GetTypeInAnotherAssembly()
        {
            Type type = TypeUtilities.GetType(new BindingEnvironment(), "System.Data.DataSet");

            Assert.IsNotNull(type);
            Assert.AreEqual(type, typeof(System.Data.DataSet));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Unknown Type 'Foo.Bar'")]
        public void RaiseIfUnknownType()
        {
            TypeUtilities.GetType(new BindingEnvironment(), "Foo.Bar");
        }

        [TestMethod]
        public void AsType()
        {
            Assert.IsNotNull(TypeUtilities.AsType("System.IO.File"));
            Assert.IsNull(TypeUtilities.AsType("Foo.Bar"));
        }

        [TestMethod]
        public void IsNamespace()
        {
            Assert.IsTrue(TypeUtilities.IsNamespace("System"));
            Assert.IsTrue(TypeUtilities.IsNamespace("PythonSharp"));
            Assert.IsTrue(TypeUtilities.IsNamespace("PythonSharp.Language"));
            Assert.IsTrue(TypeUtilities.IsNamespace("System.IO"));
            Assert.IsTrue(TypeUtilities.IsNamespace("System.Data"));

            Assert.IsFalse(TypeUtilities.IsNamespace("Foo.Bar"));
        }

        [TestMethod]
        public void GetTypesByNamespace()
        {
            var types = TypeUtilities.GetTypesByNamespace("System.IO");

            Assert.IsNotNull(types);

            Assert.IsTrue(types.Contains(typeof(System.IO.File)));
            Assert.IsTrue(types.Contains(typeof(System.IO.Directory)));
            Assert.IsTrue(types.Contains(typeof(System.IO.FileInfo)));
            Assert.IsTrue(types.Contains(typeof(System.IO.DirectoryInfo)));

            Assert.IsFalse(types.Contains(typeof(string)));
            Assert.IsFalse(types.Contains(typeof(System.Data.DataSet)));
        }

        [TestMethod]
        public void GetValueFromType()
        {
            Assert.IsFalse((bool)TypeUtilities.InvokeTypeMember(typeof(System.IO.File), "Exists", new object[] { "unknown.txt" }));
        }

        [TestMethod]
        public void GetMachineCurrent()
        {
            Assert.IsNotNull(TypeUtilities.InvokeTypeMember(typeof(Predicates), "IsFalse", new object[] { false }));
        }

        [TestMethod]
        public void GetValueFromEnum()
        {
            Assert.AreEqual(System.UriKind.RelativeOrAbsolute, TypeUtilities.InvokeTypeMember(typeof(System.UriKind), "RelativeOrAbsolute", null));
        }

        [TestMethod]
        public void ParseTokenTypeValues()
        {
            Type type = typeof(TokenType);

            Assert.AreEqual(TokenType.Name, TypeUtilities.ParseEnumValue(type, "Name"));
            Assert.AreEqual(TokenType.Integer, TypeUtilities.ParseEnumValue(type, "Integer"));
            Assert.AreEqual(TokenType.Boolean, TypeUtilities.ParseEnumValue(type, "Boolean"));
        }

        [TestMethod]
        public void RaiseWhenUnknownEnumValue()
        {
            Type type = typeof(TokenType);

            try
            {
                TypeUtilities.ParseEnumValue(type, "Spam");
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValueError));
                Assert.AreEqual("'Spam' is not a valid value of 'TokenType'", ex.Message);
            }
        }
    }
}
