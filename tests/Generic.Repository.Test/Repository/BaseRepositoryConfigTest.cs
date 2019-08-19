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
        protected readonly CacheRepository cache = new CacheRepository();

        protected BaseRepositoryAsync<TValue, TFilter> _repository;

        private DbContext dbContext;

        [SetUp]
        public void BaseUp()
        {
            dbContext = new DbInMemoryContext<TValue>(GetOptions());
            _repository = GetFakeRepository();
        }

        [TearDown]
        public void BaseTearDown()
        {
            DeleteFakeBase();
        }

        private BaseRepositoryAsync<TValue, TFilter> GetFakeRepository()
        => new BaseRepositoryAsync<TValue, TFilter>(cache, dbContext);

        private static DbContextOptions<DbInMemoryContext<TValue>> GetOptions() =>
            new DbContextOptionsBuilder<DbInMemoryContext<TValue>>()
                .UseInMemoryDatabase(databaseName: "MemoryBase")
                .Options;

        private void DeleteFakeBase() => dbContext.Database.EnsureDeleted();
    }
}
