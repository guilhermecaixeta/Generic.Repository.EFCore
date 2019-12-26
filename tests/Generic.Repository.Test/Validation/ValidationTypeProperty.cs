using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Generic.Repository.Test.Validation
{
    [TestFixture]
    public class ValidationTypeProperty
    {
        public bool IsAcceptableType(Type type)
        {
            return type.IsSubclassOf(typeof(ValueType)) ||
                   type.Equals(typeof(string)) ||
                   type.Equals(typeof(StringBuilder)) ||
                   type.Equals(typeof(StringDictionary));
        }

        [Test]
        public void TypeProperty_IsPrimitive()
        {
            Assert.IsFalse(IsAcceptableType(typeof(IEnumerable<string>)));
            Assert.IsFalse(IsAcceptableType(typeof(IEnumerable<int>)));
            Assert.IsFalse(IsAcceptableType(typeof(string[])));
            Assert.IsTrue(IsAcceptableType(typeof(string)));
            Assert.IsTrue(IsAcceptableType(typeof(StringBuilder)));
            Assert.IsTrue(IsAcceptableType(typeof(StringDictionary)));
            Assert.IsTrue(IsAcceptableType(typeof(Enum)));
            Assert.IsTrue(IsAcceptableType(typeof(double)));
            Assert.IsTrue(IsAcceptableType(typeof(double?)));
            Assert.IsTrue(IsAcceptableType(typeof(float)));
            Assert.IsTrue(IsAcceptableType(typeof(float?)));
            Assert.IsTrue(IsAcceptableType(typeof(int)));
            Assert.IsTrue(IsAcceptableType(typeof(int?)));
            Assert.IsTrue(IsAcceptableType(typeof(decimal)));
            Assert.IsTrue(IsAcceptableType(typeof(decimal?)));
            Assert.IsTrue(IsAcceptableType(typeof(DateTime)));
            Assert.IsTrue(IsAcceptableType(typeof(DateTime?)));
            Assert.IsTrue(IsAcceptableType(typeof(TimeSpan)));
            Assert.IsTrue(IsAcceptableType(typeof(TimeSpan?)));
            Assert.IsTrue(IsAcceptableType(typeof(char)));
            Assert.IsTrue(IsAcceptableType(typeof(byte)));
            Assert.IsFalse(IsAcceptableType(typeof(object)));
        }
    }
}