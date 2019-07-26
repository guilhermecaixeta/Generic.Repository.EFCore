using System;
using System.Linq.Expressions;
using Generic.Repository.Cache;
using Generic.Repository.Models.Filter;
using Generic.Repository.Repository;
using Generic.Repository.Test.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Generic.Repository.Test.Repository
{
    public abstract class BaseRepositoryAsyncTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        protected IBaseRepositoryAsync<TValue, TFilter> _repository;
        public BaseRepositoryAsyncTest()
        {
            var cacheFacade = new CacheRepositoryFacade();
            var cache = new CacheRepository(cacheFacade);

            var options = new DbContextOptionsBuilder<DbInMemoryContext<TValue>>()
                .UseInMemoryDatabase(databaseName: "MemoryBase")
                .Options;

            var context = new DbInMemoryContext<TValue>(options);
            _repository = new BaseRepositoryAsync<TValue, TFilter>(cache, context);

        }

        [Test]
        public async void Add_Valid_Value_And_Count()
        {
            var value = CreateTValue();
            await _repository.CreateAsync(value);

            var count = await _repository.CountAsync();

            Assert.AreEqual(count, 1);
        }

        [Test]
        public async void Update_Valid_Value_And_Get_Single()
        {
            var value = CreateTValue();

            await _repository.CreateAsync(value);
            await _repository.UpdateAsync(UpdateTValue(value));

            var valueUpdated = await _repository.GetSingleByAsync(ExpressionGeneric(value), true);

            Assert.AreNotSame(value, valueUpdated);
        }

        [Test]
        public async void Delete_Value()
        {
            var value = CreateTValue();
            await _repository.CreateAsync(value);
            await _repository.DeleteAsync(value);
            var count = await _repository.CountAsync();

            Assert.AreEqual(count, 0);
        }

        public abstract TValue CreateTValue();
        public abstract TValue UpdateTValue(TValue value);
        public abstract Expression<Func<TValue, bool>> ExpressionGeneric(TValue value);
    }
}
