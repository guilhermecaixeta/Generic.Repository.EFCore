namespace Generic.Repository.Test.Cache
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public abstract class CacheRepositoryExceptionTest<T>
        : CacheRepositoryInsertTest<T>
        where T : class
    {
        private const string SomeKey = "ABDC";

        private readonly string _emptyValue = string.Empty;

        [Test]
        public void GetDictionaryMethodSet_NullValueException()
        {
            Assert.Throws<ArgumentNullException>(() => Cache.GetDictionaryMethodSet(null));
        }

        [Test]
        public void GetDictionaryMethod_EmptyValueException()
        {
            Assert.Throws<ArgumentNullException>(() => Cache.GetDictionaryMethodGet(_emptyValue));
        }

        [Test]
        public void GetMethodSet_NullValueException()
        {
            Assert.Throws<ArgumentNullException>(() => Cache.GetMethodSet(null, NameProperty));
        }

        [Test]
        public void GetMethodGet_EmptyValueException()
        {
            Assert.Throws<ArgumentNullException>(() => Cache.GetMethodGet(NameType, _emptyValue));
        }

        [Test]
        public void GetDictionaryAttributes_PropertyKeyNotFoundException()
        {
            Assert.Throws<KeyNotFoundException>(() => Cache.GetMethodGet(NameType, SomeKey));
        }

        [Test]
        public void GetDictionaryAttribute_ObjectKeyNotFoundException()
        {
            Assert.Throws<KeyNotFoundException>(() => Cache.GetMethodGet(SomeKey, NameProperty));
        }
    }
}
