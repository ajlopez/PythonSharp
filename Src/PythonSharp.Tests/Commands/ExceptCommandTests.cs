namespace PythonSharp.Tests.Commands
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Expressions;
    using PythonSharp.Exceptions;

    [TestClass]
    public class ExceptCommandTests
    {
        [TestMethod]
        public void CatchValueError()
        {
            ExceptCommand command = new ExceptCommand(new ConstantExpression(typeof(ValueError)), null);

            Assert.IsNotNull(command.Expression);
            Assert.IsNull(command.Command);
            Assert.IsTrue(command.CatchException(null, new ValueError(null)));
            Assert.IsFalse(command.CatchException(null, new SyntaxError(null)));
        }

        [TestMethod]
        public void CatchBaseException()
        {
            ExceptCommand command = new ExceptCommand(new ConstantExpression(typeof(Exception)), null);

            Assert.IsNotNull(command.Expression);
            Assert.IsNull(command.Command);
            Assert.IsTrue(command.CatchException(null, new ValueError(null)));
            Assert.IsTrue(command.CatchException(null, new SyntaxError(null)));
        }

        [TestMethod]
        public void RaiseIfExpressionIsNotAnException()
        {
            ExceptCommand command = new ExceptCommand(new ConstantExpression(1), null);

            try
            {
                command.CatchException(null, new ValueError(null));
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(TypeError));
                Assert.AreEqual("catching classes that do not inherit from BaseException is not allowed", ex.Message);
            }
        }
    }
}
