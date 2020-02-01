using NUnit.Framework;
using System.Threading.Tasks;

namespace Generic.Repository.UnitTest.Cache
{
    [TestFixture]
    public abstract class CacheRepositoryGetSetValuesTest<T> : CacheConfigurationTest<T>
    where T : class
    {
        [Test]
        public async Task ClearCache_Valid()
        {
            Cache.ClearCache();
            Assert.IsFalse(await Cache.HasMethodGet(default));
            Assert.IsFalse(await Cache.HasMethodGet(default));
            Assert.IsFalse(await Cache.HasAttribute(default));
            Assert.IsFalse(await Cache.HasProperty(default));
        }

        [Test]
        public void GetAttribute_ValidValue()
        {
            var attr = Cache.GetAttribute(NameType, NameProperty, NameAttribute, default);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void GetDictionaryAttribute_ValidValue()
        {
            var attr = Cache.GetDictionaryAttribute(NameType, NameProperty, default);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void GetDictionaryAttributes_ValidValue()
        {
            var attr = Cache.GetDictionaryAttribute(NameType, default);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void GetDictionaryMethodGet_ValidValue()
        {
            var methodGet = Cache.GetDictionaryMethodGet(NameType, default);
            Assert.IsNotNull(methodGet);
        }

        [Test]
        public void GetDictionaryMethodSet_ValidValue()
        {
            var methodSet = Cache.GetDictionaryMethodSet(NameType, default);
            Assert.IsNotNull(methodSet);
        }

        [Test]
        public void GetDictionaryProperties_ValidValue()
        {
            var attr = Cache.GetDictionaryProperties(NameType, default);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void GetMethodGet_ValidValue()
        {
            var methodGet = Cache.GetMethodGet(NameType, NameProperty, default);
            Assert.IsNotNull(methodGet);
        }

        [Test]
        public void GetMethodSet_ValidValue()
        {
            var methodSet = Cache.GetMethodSet(NameType, NameProperty, default);
            Assert.IsNotNull(methodSet);
        }

        [Test]
        public void GetProperty_ValidValue()
        {
            var attr = Cache.GetProperty(NameType, NameProperty, default);
            Assert.IsNotNull(attr);
        }

        [Test]
        public async Task ValidateValues_ValidValue()
        {
            Assert.IsTrue(await Cache.HasMethodGet(default));
            Assert.IsTrue(await Cache.HasMethodSet(default));
            Assert.IsTrue(await Cache.HasProperty(default));
            Assert.IsTrue(await Cache.HasAttribute(default));
        }
    }
}