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
    public class DefinedFunctionTests
    {
        [TestMethod]
        public void CreateSimpleDefinedFunction()
        {
            IList<string> argumentNames = new string[] { "a", "b" };
            ICommand body = new SetCommand("c", new NameExpression("a"));
            DefinedFunction func = new DefinedFunction(argumentNames, body);

            Assert.AreEqual(argumentNames, func.ArgumentNames);
            Assert.AreEqual(body, func.Body);
        }
    }
}
