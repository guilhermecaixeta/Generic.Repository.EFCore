namespace Generic.Repository.Test.Cache
{
    using Generic.Repository.Cache;
    using NUnit.Framework;

    [TestFixture]
    public abstract class CacheRepositoryInsertTest<T> : CacheConfigurationTest<T>
    where T : class
    {


        [Test]
        public void ValidateValues_ValidValue()
        {
            Assert.IsTrue(Cache.HasMethodSet());
            Assert.IsTrue(Cache.HasMethodGet());
            Assert.IsTrue(Cache.HasAttribute());
            Assert.IsTrue(Cache.HasProperty());
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
        public void ClearCache_Valid()
        {
            Cache.ClearCache();
            Assert.IsFalse(Cache.HasMethodGet());
            Assert.IsFalse(Cache.HasMethodGet());
            Assert.IsFalse(Cache.HasAttribute());
            Assert.IsFalse(Cache.HasProperty());
        }
    }
}
