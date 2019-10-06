namespace Generic.Repository.Test.Cache
{
    using Generic.Repository.Cache;
    using NUnit.Framework;

    public abstract class CacheConfigurationTest<T>
        where T : class
    {
        protected ICacheRepository Cache;
        protected string NameProperty;
        protected string NameAttribute;
        protected readonly string NameType = typeof(T).Name;

        [SetUp]
        public void CacheUp()
        {
            Cache = new CacheRepository();
            Cache.AddGet<T>();
            Cache.AddSet<T>();
            Cache.AddProperty<T>();
            Cache.AddAttribute<T>();
        }

        [TearDown]
        public void CacheTearDown()
        {
            Cache.ClearCache();
        }
    }
}
