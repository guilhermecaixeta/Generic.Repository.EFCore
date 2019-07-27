namespace Generic.Repository.Test.Repository
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Generic.Repository.Cache;
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Repository;
    using Generic.Repository.Test.Data;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    [TestFixture]
    public abstract class BaseRepositoryAsyncCommandTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        protected readonly CacheRepository cache = new CacheRepository();

        protected readonly DbContext dbContext = new DbInMemoryContext<TValue>(GetOptions());

        protected readonly BaseRepositoryAsync<TValue, TFilter> _repository;

        public BaseRepositoryAsyncCommandTest()
        {
            _repository = GetRepository();
        }

        [Test]
        public async Task Create_Valid_Value_And_CountAsync()
        {
            var value = CreateTValue();
            await _repository.CreateAsync(value);
            var count = await _repository.CountAsync();

            Assert.AreEqual(1, count);
            DeleteBase();
        }

        [Test]
        public async Task Update_Valid_Value_And_Get_SingleAsync()
        {
            var value = CreateTValue();
            await _repository.CreateAsync(value);
            await _repository.UpdateAsync(UpdateTValue(value));

            var valueUpdated = await _repository.GetSingleByAsync(ExpressionGeneric(value), true);

            Assert.AreNotSame(value, valueUpdated);
        }

        [Test]
        public async Task Delete_ValueAsync()
        {
            var value = CreateTValue();
            value = await _repository.CreateAsync(value);
            await _repository.DeleteAsync(value);

            var count = await _repository.CountAsync();

            Assert.AreEqual(0, count);
            DeleteBase();
        }

        protected BaseRepositoryAsync<TValue, TFilter> GetRepository()
        => new BaseRepositoryAsync<TValue, TFilter>(cache, dbContext);
        
        protected static DbContextOptions<DbInMemoryContext<TValue>> GetOptions() => 
            new DbContextOptionsBuilder<DbInMemoryContext<TValue>>()
                .UseInMemoryDatabase(databaseName: "MemoryBase")
                .Options;

        protected void DeleteBase() => dbContext.Database.EnsureDeleted();
        protected abstract TValue CreateTValue();
        protected abstract TValue UpdateTValue(TValue value);
        protected abstract Expression<Func<TValue, bool>> ExpressionGeneric(TValue value);
    }
}
