namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Cache;
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Repository;
    using Generic.Repository.Test.Data;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    public abstract class BaseRepositoryConfigTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        protected readonly CacheRepository Cache = new CacheRepository();

        protected BaseRepositoryAsync<TValue, TFilter> Repository;

        private DbContext _dbContext;

        [SetUp]
        public void BaseUp()
        {
            var contextOptions = GetDbContextOptionsFake();
            _dbContext = new DbInMemoryContext<TValue>(contextOptions);
            Repository = GetRepositoryFake();
        }

        [TearDown]
        public void BaseTearDown() =>
            _dbContext.Database.EnsureDeleted();

        private BaseRepositoryAsync<TValue, TFilter> GetRepositoryFake() =>
            new BaseRepositoryAsync<TValue, TFilter>(Cache, _dbContext);

        private static DbContextOptions<DbInMemoryContext<TValue>> GetDbContextOptionsFake() =>
            new DbContextOptionsBuilder<DbInMemoryContext<TValue>>()
                .UseInMemoryDatabase(databaseName: "MemoryBase")
                .Options;

    }
}
