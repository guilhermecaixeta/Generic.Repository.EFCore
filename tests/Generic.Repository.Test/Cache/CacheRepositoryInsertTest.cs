namespace Generic.Repository.Test.Cache
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public abstract class CacheRepositoryInsertTest<T> : CacheConfigurationTest<T>
    where T : class
    {


        [Test]
        public async Task ValidateValues_ValidValue()
        {
            Assert.IsTrue(await Cache.HasMethodGet());
            Assert.IsTrue(await Cache.HasMethodSet());
            Assert.IsTrue(await Cache.HasProperty());
            Assert.IsTrue(await Cache.HasAttribute());
        }

        [Test]
        public void GetDictionaryMethodSet_ValidValue()
        {
            var methodSet = Cache.GetDictionaryMethodSet(NameType);
            Assert.IsNotNull(methodSet);
        }

        [Test]
        public void GetDictionaryMethodGet_ValidValue()
        {
            var methodGet = Cache.GetDictionaryMethodGet(NameType);
            Assert.IsNotNull(methodGet);
        }

        [Test]
        public void GetMethodSet_ValidValue()
        {
            var methodSet = Cache.GetMethodSet(NameType, NameProperty);
            Assert.IsNotNull(methodSet);
        }

        [Test]
        public void GetMethodGet_ValidValue()
        {
            var methodGet = Cache.GetMethodGet(NameType, NameProperty);
            Assert.IsNotNull(methodGet);
        }

        [Test]
        public void GetDictionaryAttributes_ValidValue()
        {
            var attr = Cache.GetDictionaryAttribute(NameType);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void GetDictionaryAttribute_ValidValue()
        {
            var attr = Cache.GetDictionaryAttribute(NameType, NameProperty);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void GetAttribute_ValidValue()
        {
            var attr = Cache.GetAttribute(NameType, NameProperty, NameAttribute);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void GetDictionaryProperties_ValidValue()
        {
            var attr = Cache.GetDictionaryProperties(NameType);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void GetProperty_ValidValue()
        {
            var attr = Cache.GetProperty(NameType, NameProperty);
            Assert.IsNotNull(attr);
        }

        [Test]
        public async Task ClearCache_Valid()
        {
            Cache.ClearCache();
            Assert.IsFalse(await Cache.HasMethodGet());
            Assert.IsFalse(await Cache.HasMethodGet());
            Assert.IsFalse(await Cache.HasAttribute());
            Assert.IsFalse(await Cache.HasProperty());
        }
    }
}
