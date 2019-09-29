﻿namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Models.Page.PageConfig;
    using Generic.Repository.Test.Model.Filter;
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
        private const int Zero = 0;

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
        public async Task CountAsync_DataValid()
        {
            var count = await Repository.CountAsync();

            Assert.AreEqual(ComparableListLength, count);
        }

        [Test]
        public async Task CountAsync_WithPredicate_DataValid()
        {
            var count = await Repository.CountAsync(GetFakeExpression());

            Assert.AreEqual(ComparablePageFilterResult, count);
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
            var filter = GetFilterFake();
            var list = await Repository.FilterAllAsync(filter, true);
            Assert.IsNotNull(list);

            Assert.AreEqual(ComparablePageFilterResult, list.Count);

            filter = UpdateFilter();
            list = await Repository.FilterAllAsync(filter, true);
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
            Assert.AreEqual(Zero, page.TotalPage);
        }

        internal abstract TFilter UpdateFilter();

        internal abstract IPageConfig GetPageConfigFake();

        internal abstract TFilter GetFilterFake();

        internal abstract Expression<Func<TValue, bool>> GetFakeExpression();
    }
}
