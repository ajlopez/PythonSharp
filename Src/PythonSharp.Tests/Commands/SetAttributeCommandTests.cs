namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;
    using PythonSharp.Language;
    using PythonSharp.Tests.Classes;

    [TestClass]
    public class SetAttributeCommandTests
    {
        [TestMethod]
        public void SetAttributeInDynamicObject()
        {
            BindingEnvironment environment = new BindingEnvironment();
            DefinedClass klass = new DefinedClass("Spam");
            DynamicObject dynobj = new DynamicObject(klass);
            environment.SetValue("foo", dynobj);

            SetAttributeCommand command = new SetAttributeCommand(new NameExpression("foo"), "one", new ConstantExpression(1));

            command.Execute(environment);

            Assert.IsTrue(dynobj.HasValue("one"));
            Assert.AreEqual(1, dynobj.GetValue("one"));
        }

        [TestMethod]
        public void SetAttributeInNativeObject()
        {
            BindingEnvironment environment = new BindingEnvironment();
            Person adam = new Person();
            environment.SetValue("adam", adam);

            SetAttributeCommand command = new SetAttributeCommand(new NameExpression("adam"), "FirstName", new ConstantExpression("Adam"));

            command.Execute(environment);

            Assert.AreEqual("Adam", adam.FirstName);
        }
    }
}
