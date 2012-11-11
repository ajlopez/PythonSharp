namespace PythonSharp.Tests.Language
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Exceptions;
    using PythonSharp.Language;

    [TestClass]
    public class RangeTests
    {
        [TestMethod]
        public void CreateSimpleRange()
        {
            Range range = new Range(2);

            var list = range.ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(0, list[0]);
            Assert.AreEqual(1, list[1]);
        }

        [TestMethod]
        public void CreateRangeFromTo()
        {
            Range range = new Range(2, 4);

            var list = range.ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(2, list[0]);
            Assert.AreEqual(3, list[1]);
        }

        [TestMethod]
        public void CreateRangeFromToStep()
        {
            Range range = new Range(2, 6, 2);

            var list = range.ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(2, list[0]);
            Assert.AreEqual(4, list[1]);
        }

        [TestMethod]
        public void CreateRangeFromToNegativeStep()
        {
            Range range = new Range(6, 2, -2);

            var list = range.ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(6, list[0]);
            Assert.AreEqual(4, list[1]);
        }

        [TestMethod]
        public void CreateEmptyRangeFromTo()
        {
            Range range = new Range(2, -4);

            var list = range.ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void CreateSimpleEmptyRange()
        {
            Range range = new Range(0);

            var list = range.ToList();

            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void RaiseIsStepIsZero()
        {
            try
            {
                new Range(1, 2, 0);
                Assert.Fail("Exception expected");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ValueError));
                Assert.AreEqual("range() arg 3 must not be zero", ex.Message);
            }
        }
    }
}
