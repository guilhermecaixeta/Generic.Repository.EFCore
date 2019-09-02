namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Models.Page.PageConfig;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    [TestFixture]
    public abstract class BaseRepositoryAsyncQueryTest<TValue, TFilter>
        : BaseRepositoryExceptionTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        protected int ComparableListLength;

        protected int ComparablePageLength;

        protected int ComparablePageFilterResult;

        [SetUp]
        public async Task FakeDataUp()
        {
            var mockList = GetListFake();
            await Repository.CreateAsync(mockList);
        }

        [Test]
        public async Task GetAllAsync_DataValid()
        {
            var list = await Repository.GetAllAsync(true);

            Assert.IsNotNull(list);
            Assert.AreEqual(ComparableListLength, list.Count);
        }

        [Test]
        public async Task GetAllByAsync_DataValid()
        {
            var list = await Repository.GetAllByAsync(GetFakeExpression(), true);
            var result = list.Count;
            Assert.IsNotNull(list);
            Assert.AreEqual(ComparablePageFilterResult, result);
        }

        [Test]
        public async Task FilterAllAsync_DataValid()
        {
            var list = await Repository.FilterAllAsync(GetFakeFilter(), true);

            Assert.IsNotNull(list);

            Assert.AreEqual(ComparablePageFilterResult, list.Count);
        }

        [Test]
        public async Task FirstAsync_DataValid()
        {
            var value = await Repository.GetFirstByAsync(GetFakeExpression(), true);

            Assert.IsNotNull(value);
        }

        [Test]
        public async Task PageAllAsync_DataValid()
        {
            var page = await Repository.GetPageAsync(GetFakePageConfig(), true);

            Assert.IsNotNull(page.Content);
            Assert.AreEqual(ComparablePageLength, page.Content.Count);
        }

        [Test]
        public async Task PageAll_FilterByExpressionAsync__DataValid()
        {
            var page = await Repository.GetPageAsync(GetFakePageConfig(), GetFakeExpression(), true);

            Assert.IsNotNull(page);
            Assert.AreEqual(ComparablePageFilterResult, page.Content.Count);
        }

        [Test]
        public async Task PageAll_FilterByFilterDefaultAsync_DataValid()
        {
            var page = await Repository.GetPageAsync(GetFakePageConfig(), GetFakeFilter(), true);

            Assert.IsNotNull(page.Content);
            Assert.AreEqual(ComparablePageFilterResult, page.Content.Count);
        }

        internal abstract IEnumerable<TValue> GetListFake();
        internal abstract IPageConfig GetFakePageConfig();
        internal abstract TFilter GetFakeFilter();
        internal abstract Expression<Func<TValue, bool>> GetFakeExpression();
    }
}
