namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Language;

    [TestClass]
    public class ClassCommandTests
    {
        [TestMethod]
        public void ExecuteSimpleClassCommand()
        {
            BindingEnvironment environment = new BindingEnvironment();
            ClassCommand command = new ClassCommand("Spam", new PassCommand());

            command.Execute(environment);

            var result = environment.GetValue("Spam");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DefinedClass));

            var dclass = (DefinedClass)result;

            Assert.AreEqual("Spam", dclass.Name);
        }
    }
}
