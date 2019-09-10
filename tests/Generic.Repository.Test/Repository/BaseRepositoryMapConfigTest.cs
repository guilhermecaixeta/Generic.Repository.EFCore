using NUnit.Framework;

namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Repository;
    using Generic.Repository.Test.Repository.Commom;
    using System.Collections.Generic;

    public abstract class BaseRepositoryMapConfigTest<TValue, TResult, TFilter> :
        BaseRepositoryCommomConfig<TValue>
        where TValue : class
        where TResult : class
        where TFilter : class, IFilter
    {
        [SetUp]
        public void ChildBaseUp() =>
            Repository = GetRepositoryFake();


        protected BaseRepositoryAsync<TValue, TResult, TFilter> Repository;

        private BaseRepositoryAsync<TValue, TResult, TFilter> GetRepositoryFake() =>
            new BaseRepositoryAsync<TValue, TResult, TFilter>(Cache, DbContext, MapperList, MapperDate);

        protected abstract TResult MapperDate(TValue value);

        protected abstract IEnumerable<TResult> MapperList(IEnumerable<TValue> value);

    }
}
