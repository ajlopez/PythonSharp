namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Language;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;

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
    }
}
