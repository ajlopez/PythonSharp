using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AjPython.Tests
{
    [TestClass]
    public class PredicatesTests
    {
        [TestMethod]
        public void IsFalse()
        {
            Assert.IsTrue(Predicates.IsFalse(null));
            Assert.IsTrue(Predicates.IsFalse(false));
            Assert.IsTrue(Predicates.IsFalse(0));
            Assert.IsTrue(Predicates.IsFalse(0.0));
            Assert.IsTrue(Predicates.IsFalse(""));
        }

        [TestMethod]
        public void IsNotFalse()
        {
            Assert.IsFalse(Predicates.IsFalse(" "));
            Assert.IsFalse(Predicates.IsFalse(true));
            Assert.IsFalse(Predicates.IsFalse(1));
            Assert.IsFalse(Predicates.IsFalse(0.1));
            Assert.IsFalse(Predicates.IsFalse("1"));
            Assert.IsFalse(Predicates.IsFalse("0"));
        }
    }
}
