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
        protected readonly string NameType = typeof(T).Name;

        [SetUp]
        public async Task CacheUp()
        {
            Cache = new CacheRepository();
            await Cache.AddGet<T>();
            await Cache.AddSet<T>();
            await Cache.AddProperty<T>();
            await Cache.AddAttribute<T>();
        }

        [TearDown]
        public void CacheTearDown()
        {
            Cache.ClearCache();
        }
    }
}
