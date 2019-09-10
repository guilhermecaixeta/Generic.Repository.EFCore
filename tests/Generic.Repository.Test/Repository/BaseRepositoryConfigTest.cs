namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Repository;
    using Generic.Repository.Test.Repository.Commom;
    using NUnit.Framework;

    public abstract class BaseRepositoryConfigTest<TValue, TFilter> :
        BaseRepositoryCommomConfig<TValue>
        where TValue : class
        where TFilter : class, IFilter
    {

        protected BaseRepositoryAsync<TValue, TFilter> Repository;

        [SetUp]
        public void ChildBaseUp() =>
            Repository = GetRepositoryFake();


        private BaseRepositoryAsync<TValue, TFilter> GetRepositoryFake() =>
            new BaseRepositoryAsync<TValue, TFilter>(Cache, DbContext);

    }
}
