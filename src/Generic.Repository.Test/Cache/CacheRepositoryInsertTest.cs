using Generic.Repository.Cache;
using NUnit.Framework;

namespace Generic.Repository.Test.Cache
{
    [TestFixture]
    public abstract class CacheRepositoryInsertTest<T>
    where T : class
    {
        protected ICacheRepository cache;
        protected string nameProperty;
        protected string nameAttribute;

        [Test]
        public void Add_Valid_Class_And_Verify_If_Has_Value()
        {
            cache.Add<T>();
            Assert.IsTrue(cache.HasMethodSet());
            Assert.IsTrue(cache.HasMethodGet());
            Assert.IsTrue(cache.HasAttribute());
            Assert.IsTrue(cache.HasProperty());
        }

        [Test]
        public void Get_Dictionary_Method_Set()
        {
            InitCache();
            var methodSet = cache.GetDictionaryMethodSet(typeof(T).Name);
            Assert.IsNotNull(methodSet);
        }

        [Test]
        public void Get_Dictionary_Method_Get()
        {
            InitCache();
            var methodGet = cache.GetDictionaryMethodGet(typeof(T).Name);
            Assert.IsNotNull(methodGet);
        }

        [Test]
        public void Get_Method_Set()
        {
            InitCache();
            var methodSet = cache.GetMethodSet(typeof(T).Name, nameProperty);
            Assert.IsNotNull(methodSet);
        }

        [Test]
        public void Get_Method_Get()
        {
            InitCache();
            var methodGet = cache.GetMethodGet(typeof(T).Name, nameProperty);
            Assert.IsNotNull(methodGet);
        }

        [Test]
        public void Get_Dictionary_Attributes()
        {
            InitCache();
            var attr = cache.GetDictionaryAttribute(typeof(T).Name);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void Get_Dictionary_Attribute()
        {
            InitCache();
            var attr = cache.GetDictionaryAttribute(typeof(T).Name, nameProperty);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void Get_Attribute()
        {
            InitCache();
            var attr = cache.GetAttribute(typeof(T).Name, nameProperty, nameAttribute);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void Get_Dictionary_Properties()
        {
            InitCache();
            var attr = cache.GetDictionaryProperties(typeof(T).Name);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void Get_Property()
        {
            InitCache();
            var attr = cache.GetProperty(typeof(T).Name, nameProperty);
            Assert.IsNotNull(attr);
        }

        [Test]
        public void Clear_Cache()
        {
            cache.ClearCache();
            Assert.IsFalse(cache.HasMethodGet());
        }

        protected void InitCache() => cache.Add<T>();
    }
}
