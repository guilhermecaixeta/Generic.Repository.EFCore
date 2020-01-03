using System.Collections.Generic;
using Generic.Repository.Models.Filter;
using Generic.Repository.Repository;
using Generic.Repository.UnitTest.Data;
using Generic.Repository.UnitTest.Repository.Commom;
using NUnit.Framework;

namespace Generic.Repository.UnitTest.Repository
{
    public abstract class BaseRepositoryMapConfigTest<TValue, TResult, TFilter> :
        BaseRepositoryCommomConfig<TValue>
        where TValue : class
        where TResult : class
        where TFilter : class, IFilter
    {
        protected BaseRepositoryAsync<TValue, TFilter, DbInMemoryContext<TValue>> Repository;

        [SetUp]
        public void ChildBaseUp() =>
            Repository = GetRepositoryFake();

        protected abstract TResult MapperDate(TValue value);

        protected abstract IEnumerable<TResult> MapperList(IEnumerable<TValue> value);

        protected abstract TValue MapperReturnToDate(TResult value);

        private BaseRepositoryAsync<TValue, TFilter, DbInMemoryContext<TValue>> GetRepositoryFake() =>
                                    new BaseRepositoryAsync<TValue, TFilter, DbInMemoryContext<TValue>>(DbContext, Cache);
    }
}