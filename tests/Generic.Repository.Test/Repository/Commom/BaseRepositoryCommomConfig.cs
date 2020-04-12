using Generic.Repository.Cache;
using Generic.RepositoryTest.Unit.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Generic.RepositoryTest.Unit.Repository.Commom
{
    public abstract class BaseRepositoryCommomConfig<TValue>
    where TValue : class
    {
        protected readonly ICacheRepository Cache = new CacheRepository();

        protected DbInMemoryContext<TValue> DbContext;

        [TearDown]
        public virtual void BaseTearDown() =>
            DbContext.Database.EnsureDeleted();

        [SetUp]
        public virtual void BaseUp()
        {
            var contextOptions = GetDbContextOptionsFake();
            DbContext = new DbInMemoryContext<TValue>(contextOptions);
        }

        private static DbContextOptions<DbInMemoryContext<TValue>> GetDbContextOptionsFake() =>
            new DbContextOptionsBuilder<DbInMemoryContext<TValue>>()
                .UseInMemoryDatabase("MemoryBase")
                .Options;
    }
}