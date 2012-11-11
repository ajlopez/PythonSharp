namespace PythonSharp.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.IsTrue(Predicates.IsFalse(string.Empty));
        }

        [TestMethod]
        public void SequencesAsBooleans()
        {
            Assert.IsTrue(Predicates.IsFalse(string.Empty));
            Assert.IsTrue(Predicates.IsFalse(new ArrayList()));
            Assert.IsTrue(Predicates.IsFalse(new List<object>() { }));

            Assert.IsFalse(Predicates.IsFalse(" "));
            Assert.IsFalse(Predicates.IsFalse(new List<object>() { 1 }));
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
