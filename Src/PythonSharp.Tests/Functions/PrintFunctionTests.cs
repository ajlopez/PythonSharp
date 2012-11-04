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
            var print = new PrintFunction();
            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            var result = print.Apply(machine.Environment, new object[] { "bar" });

            Assert.IsNull(result);
            Assert.AreEqual("bar\r\n", writer.ToString());
        }

        [TestMethod]
        public void EvaluatePrintFunctionWithTwoValues()
        {
            var print = new PrintFunction();
            Machine machine = new Machine();
            StringWriter writer = new StringWriter();
            machine.Output = writer;

            var result = print.Apply(machine.Environment, new object[] { "bar", "foo" });

            Assert.IsNull(result);

            Assert.AreEqual("bar foo\r\n", writer.ToString());
        }
    }
}
