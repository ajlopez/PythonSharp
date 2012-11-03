namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;

    [TestClass]
    public class DefCommandTests
    {
        [TestMethod]
        public void CreateSimpleDefCommand()
        {
            IList<string> argumentNames = new string[] { "a", "b" };
            ICommand body = new SetCommand("c", new ConstantExpression(1));
            DefCommand command = new DefCommand("foo", argumentNames, body);

            Assert.AreEqual("foo", command.Name);
            Assert.AreEqual(argumentNames, command.ArgumentNames);
            Assert.AreEqual(body, command.Body);
        }
    }
}
