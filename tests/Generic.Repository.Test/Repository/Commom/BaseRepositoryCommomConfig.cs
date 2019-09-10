namespace Generic.Repository.Test.Repository.Commom
{
    using Generic.Repository.Cache;
    using Generic.Repository.Test.Data;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    public abstract class BaseRepositoryCommomConfig<TValue>
    where TValue : class
    {
        protected readonly CacheRepository Cache = new CacheRepository();

        protected DbContext DbContext;

        [SetUp]
        public virtual void BaseUp()
        {
            var contextOptions = GetDbContextOptionsFake();
            DbContext = new DbInMemoryContext<TValue>(contextOptions);
        }

        [TearDown]
        public virtual void BaseTearDown() =>
            DbContext.Database.EnsureDeleted();

        private static DbContextOptions<DbInMemoryContext<TValue>> GetDbContextOptionsFake() =>
            new DbContextOptionsBuilder<DbInMemoryContext<TValue>>()
                .UseInMemoryDatabase(databaseName: "MemoryBase")
                .Options;
    }
}
