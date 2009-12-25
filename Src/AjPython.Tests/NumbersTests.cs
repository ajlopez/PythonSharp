namespace AjPython.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NumbersTests
    {
        [TestMethod]
        public void CalculateSimpleGCD()
        {
            Assert.AreEqual(1, Numbers.GreatestCommonDivisor(3, 1));
            Assert.AreEqual(2, Numbers.GreatestCommonDivisor(4, 2));
            Assert.AreEqual(3, Numbers.GreatestCommonDivisor(9, 6));
        }

        [TestMethod]
        public void CalculateAbs() 
        {
            Assert.AreEqual(0, Numbers.Abs(0));
            Assert.AreEqual(1, Numbers.Abs(1));
            Assert.AreEqual(2, Numbers.Abs(-2));
            Assert.AreEqual(1.23, Numbers.Abs(1.23));
            Assert.AreEqual(2.34, Numbers.Abs(-2.34));
        }

        [TestMethod]
        public void IsFixnum()
        {
            Assert.IsTrue(Numbers.IsFixnum(1));
            Assert.IsTrue(Numbers.IsFixnum(2L));
            Assert.IsTrue(Numbers.IsFixnum((short)3));

            Assert.IsFalse(Numbers.IsFixnum(null));
            Assert.IsFalse(Numbers.IsFixnum("foo"));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(7, Numbers.Add(5, 2));
            Assert.AreEqual(5.3 - 2.1, Numbers.Add(5.3, -2.1));
        }

        [TestMethod]
        public void AddAsConcatenate()
        {
            Assert.AreEqual("Hello world", Numbers.Add("Hello " , "world"));
            Assert.AreEqual("Message 1", Numbers.Add("Message ", 1));
            Assert.AreEqual("1 Message", Numbers.Add(1, " Message"));
        }

        [TestMethod]
        public void Subtract()
        {
            Assert.AreEqual(3, Numbers.Subtract(5, 2));
            Assert.AreEqual(5.3 - 2.1, Numbers.Subtract(5.3, 2.1));
        }

        [TestMethod]
        public void Multiply()
        {
            Assert.AreEqual(2, Numbers.Multiply(1, 2));
            Assert.AreEqual(2.2, Numbers.Multiply(1.1, 2));
        }

        [TestMethod]
        public void Divide()
        {
            Assert.AreEqual(0, Numbers.Divide(1, 2));
            Assert.AreEqual(2, Numbers.Divide(4, 2));
            Assert.AreEqual(4 / 3.0, Numbers.Divide(4, 3.0));
        }
    }
}
