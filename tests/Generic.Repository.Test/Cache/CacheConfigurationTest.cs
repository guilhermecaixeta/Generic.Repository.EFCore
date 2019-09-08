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

        [SetUp]
        public void CacheUp()
        {
            Cache = new CacheRepository();
            Cache.Add<T>();
        }

        [TearDown]
        public void CacheTearDown()
        {
            Cache.ClearCache();
        }
    }
}
