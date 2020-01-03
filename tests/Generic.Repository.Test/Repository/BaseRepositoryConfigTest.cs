using Generic.Repository.Models.Filter;
using Generic.Repository.Repository;
using Generic.Repository.UnitTest.Data;
using Generic.Repository.UnitTest.Repository.Commom;
using NUnit.Framework;

namespace Generic.Repository.UnitTest.Repository
{
    public abstract class BaseRepositoryConfigTest<TValue, TFilter> :
        BaseRepositoryCommomConfig<TValue>
        where TValue : class
        where TFilter : class, IFilter
    {
        protected BaseRepositoryAsync<TValue, TFilter, DbInMemoryContext<TValue>> Repository;

        [SetUp]
        public void ChildBaseUp() =>
            Repository = GetRepositoryFake();

        private BaseRepositoryAsync<TValue, TFilter, DbInMemoryContext<TValue>> GetRepositoryFake() =>
            new BaseRepositoryAsync<TValue, TFilter, DbInMemoryContext<TValue>>(DbContext, Cache);
    }
}