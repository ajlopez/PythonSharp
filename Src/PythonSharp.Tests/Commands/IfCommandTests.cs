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
    public class IfCommandTests
    {
        [TestMethod]
        public void ExecuteIfCommandWithTrueCondition()
        {
            IfCommand ifcmd = new IfCommand(new ConstantExpression(true), new SetCommand("one", new ConstantExpression(1)));
            Machine machine = new Machine();

            ifcmd.Execute(machine, machine.Environment);

            Assert.IsNotNull(ifcmd.ThenCommand);
            Assert.IsNull(ifcmd.ElseCommand);
            Assert.AreEqual(1, machine.Environment.GetValue("one"));
        }

        [TestMethod]
        public void ExecuteIfCommandWithFalseCondition()
        {
            IfCommand ifcmd = new IfCommand(new ConstantExpression(false), new SetCommand("one", new ConstantExpression(1)));
            Machine machine = new Machine();

            ifcmd.Execute(machine, machine.Environment);

            Assert.IsNull(machine.Environment.GetValue("one"));
        }

        [TestMethod]
        public void ExecuteIfElseCommandWithFalseCondition()
        {
            IfCommand ifcmd = new IfCommand(new ConstantExpression(false), new SetCommand("one", new ConstantExpression(1)), new SetCommand("two", new ConstantExpression(2)));
            Machine machine = new Machine();

            ifcmd.Execute(machine, machine.Environment);

            Assert.IsNull(machine.Environment.GetValue("one"));
            Assert.AreEqual(2, machine.Environment.GetValue("two"));
        }
    }
}
