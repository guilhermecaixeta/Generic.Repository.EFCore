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
        : BaseRepositoryAsyncCommandTest<TValue, TFilter>
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
            var list = await Repository.FilterAllAsync(GetFilterFake(), true);

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
            var page = await Repository.GetPageAsync(GetPageConfigFake(), true);

            Assert.IsNotNull(page.Content);
            Assert.AreEqual(ComparablePageLength, page.Content.Count);
        }

        [Test]
        public async Task PageAll_FilterByExpressionAsync__DataValid()
        {
            var page = await Repository.GetPageAsync(GetPageConfigFake(), GetFakeExpression(), true);

            Assert.IsNotNull(page);
            Assert.AreEqual(ComparablePageFilterResult, page.Content.Count);
        }

        [Test]
        public async Task PageAll_FilterByFilterDefaultAsync_DataValid()
        {
            var page = await Repository.GetPageAsync(GetPageConfigFake(), GetFilterFake(), true);

            Assert.IsNotNull(page.Content);
            Assert.AreEqual(ComparablePageFilterResult, page.Content.Count);
        }

        [Test]
        public async Task PageAll_NoData()
        {
            BaseTearDown();
            var page = await Repository.GetPageAsync(GetPageConfigFake(), GetFilterFake(), true);

            Assert.AreEqual(new List<TValue>(), page.Content);
            Assert.AreEqual(0, page.TotalPage);
        }

        internal abstract IPageConfig GetPageConfigFake();
        internal abstract TFilter GetFilterFake();
        internal abstract Expression<Func<TValue, bool>> GetFakeExpression();
    }
}
