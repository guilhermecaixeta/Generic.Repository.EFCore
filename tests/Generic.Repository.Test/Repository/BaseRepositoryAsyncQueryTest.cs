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
        : BaseRespositoryExceptionTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        protected int ComparableListLength;

        protected int ComparablePageLength;

        protected int ComparablePageFilterResult;

        [SetUp]
        public async Task FakeQueryDataUp()
        {
            var mockList = GetListFake();
            await _repository.CreateAsync(mockList);
        }

        [Test]
        public async Task Get_All_DataAsync()
        {
            var list = await _repository.GetAllAsync(true);

            Assert.IsNotNull(list);
            Assert.AreEqual(ComparableListLength, list.Count);
        }

        [Test]
        public async Task Get_All_By_DataAsync()
        {
            var list = await _repository.GetAllByAsync(GetFakeExpression(), true);
            var result = list.Count;
            Assert.IsNotNull(list);
            Assert.AreEqual(ComparablePageFilterResult, result);
        }

        [Test]
        public async Task Filter_All_DataAsync()
        {
            var list = await _repository.FilterAllAsync(GetFakeFilter(), true);

            Assert.IsNotNull(list);

            Assert.AreEqual(ComparablePageFilterResult, list.Count);
        }

        [Test]
        public async Task First_DataAsync()
        {
            var value = await _repository.GetFirstByAsync(GetFakeExpression(), true);

            Assert.IsNotNull(value);
        }

        [Test]
        public async Task Page_All_DataAsync()
        {
            var page = await _repository.GetPageAsync(GetFakePageConfig(), true);

            Assert.IsNotNull(page.Content);
            Assert.AreEqual(ComparablePageLength, page.Content.Count);
        }

        [Test]
        public async Task Page_All_Expression_DataAsync()
        {
            var page = await _repository.GetPageAsync(GetFakePageConfig(), GetFakeExpression(), true);

            Assert.IsNotNull(page);
            Assert.AreEqual(ComparablePageFilterResult, page.Content.Count);
        }

        [Test]
        public async Task Page_All_Filter_DataAsync()
        {
            var page = await _repository.GetPageAsync(GetFakePageConfig(), GetFakeFilter(), true);

            Assert.IsNotNull(page.Content);
            Assert.AreEqual(ComparablePageFilterResult, page.Content.Count);
        }

        internal abstract IEnumerable<TValue> GetListFake();
        internal abstract IPageConfig GetFakePageConfig();
        internal abstract TFilter GetFakeFilter();
        internal abstract Expression<Func<TValue, bool>> GetFakeExpression();
    }
}
