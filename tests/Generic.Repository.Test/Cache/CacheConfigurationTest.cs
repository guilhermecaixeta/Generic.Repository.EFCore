namespace Generic.Repository.Test.Cache
{
    using Generic.Repository.Cache;
    using NUnit.Framework;
    using System.Threading.Tasks;

    public abstract class CacheConfigurationTest<T>
        where T : class
    {
        protected ICacheRepository Cache;
        protected string NameProperty;
        protected string NameAttribute;
        protected string SomeKey = "ABDC";
        protected string NoCacheableProperty;
        protected readonly string NameType = typeof(T).Name;

        [SetUp]
        public async Task CacheUp()
        {
            Cache = new CacheRepository();
            await Cache.AddGet<T>(default);
            await Cache.AddSet<T>(default);
            await Cache.AddProperty<T>(default);
            await Cache.AddAttribute<T>(default);
        }

        [TearDown]
        public void CacheTearDown()
        {
            Cache.ClearCache();
        }
    }
}
