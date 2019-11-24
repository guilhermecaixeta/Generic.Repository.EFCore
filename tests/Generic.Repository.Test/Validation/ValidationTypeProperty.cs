using NUnit.Framework;
using System;
using System.Collections.Specialized;
using System.Text;

namespace Generic.Repository.Test.Validation
{
    [TestFixture]
    public class ValidationTypeProperty
    {
        [Test]
        public void TypeProperty_IsPrimitive()
        {
            Assert.IsTrue(IsPrimitive(typeof(string)));
            Assert.IsTrue(IsPrimitive(typeof(StringBuilder)));
            Assert.IsTrue(IsPrimitive(typeof(StringDictionary)));
            Assert.IsTrue(IsPrimitive(typeof(Enum)));
            Assert.IsTrue(IsPrimitive(typeof(double)));
            Assert.IsTrue(IsPrimitive(typeof(double?)));
            Assert.IsTrue(IsPrimitive(typeof(float)));
            Assert.IsTrue(IsPrimitive(typeof(float?)));
            Assert.IsTrue(IsPrimitive(typeof(int)));
            Assert.IsTrue(IsPrimitive(typeof(int?)));
            Assert.IsTrue(IsPrimitive(typeof(decimal)));
            Assert.IsTrue(IsPrimitive(typeof(decimal?)));
            Assert.IsTrue(IsPrimitive(typeof(DateTime)));
            Assert.IsTrue(IsPrimitive(typeof(DateTime?)));
            Assert.IsTrue(IsPrimitive(typeof(TimeSpan)));
            Assert.IsTrue(IsPrimitive(typeof(TimeSpan?)));
            Assert.IsTrue(IsPrimitive(typeof(char)));
            Assert.IsTrue(IsPrimitive(typeof(byte)));
            Assert.IsFalse(IsPrimitive(typeof(object)));
        }

        public bool IsPrimitive(Type type)
        {
            return type.IsSubclassOf(typeof(ValueType)) ||
                    type.Equals(typeof(string)) ||
                    type.Equals(typeof(StringBuilder)) ||
                    type.Equals(typeof(StringDictionary));
        }
    }
}
