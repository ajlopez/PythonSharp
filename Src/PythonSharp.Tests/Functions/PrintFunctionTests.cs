namespace PythonSharp.Tests.Functions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Functions;
    using System.IO;

    [TestClass]
    public class PrintFunctionTests
    {
        [TestMethod]
        public void EvaluatePrintFunction()
        {
            Machine machine = new Machine();
            var print = new PrintFunction(machine);
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            var result = print.Apply(machine.Environment, new object[] { "bar" }, null);

            Assert.IsNull(result);
            Assert.AreEqual("bar\r\n", writer.ToString());
        }

        [TestMethod]
        public void EvaluatePrintFunctionWithTwoValues()
        {
            Machine machine = new Machine();
            var print = new PrintFunction(machine);
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            var result = print.Apply(machine.Environment, new object[] { "bar", "foo" }, null);

            Assert.IsNull(result);

            Assert.AreEqual("bar foo\r\n", writer.ToString());
        }
    }
}
