namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;
    using System.Collections;

    [TestClass]
    public class SetIndexCommandTests
    {
        [TestMethod]
        public void CreateSetIndexCommand()
        {
            IExpression targetExpression = new ConstantExpression(1);
            IExpression indexExpression = new ConstantExpression(2);
            IExpression valueExpression = new ConstantExpression(3);

            var command = new SetIndexCommand(targetExpression, indexExpression, valueExpression);

            Assert.AreEqual(targetExpression, command.TargetExpression);
            Assert.AreEqual(indexExpression, command.IndexExpression);
            Assert.AreEqual(valueExpression, command.Expression);
        }

        [TestMethod]
        public void ExecuteSetIndexCommandOnArray()
        {
            var array = new object[] { 1, 2, 2 };
            IExpression targetExpression = new ConstantExpression(array);
            IExpression indexExpression = new ConstantExpression(2);
            IExpression valueExpression = new ConstantExpression(3);

            var command = new SetIndexCommand(targetExpression, indexExpression, valueExpression);
            command.Execute(null);

            Assert.AreEqual(3, array[2]);
        }

        [TestMethod]
        public void ExecuteSetIndexCommandOnList()
        {
            var list = new List<object>() { 1, 2, 2 };
            IExpression targetExpression = new ConstantExpression(list);
            IExpression indexExpression = new ConstantExpression(2);
            IExpression valueExpression = new ConstantExpression(3);

            var command = new SetIndexCommand(targetExpression, indexExpression, valueExpression);
            command.Execute(null);

            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void ExecuteSetIndexCommandOnDictionary()
        {
            var dictionary = new Hashtable();
            IExpression targetExpression = new ConstantExpression(dictionary);
            IExpression indexExpression = new ConstantExpression("foo");
            IExpression valueExpression = new ConstantExpression("bar");

            var command = new SetIndexCommand(targetExpression, indexExpression, valueExpression);
            command.Execute(null);

            Assert.AreEqual("bar", dictionary["foo"]);
        }
    }
}
