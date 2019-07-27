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
        protected int count;
        protected int count2;
        protected int count3;

        protected abstract IEnumerable<TValue> GetListSimpleObject();

        [Test]
        public async Task Get_All_DataAsync()
        {
            SetEnvironment();
            var list = await _repository.GetAllAsync(true);

            Assert.IsNotNull(list);
            Assert.AreEqual(count, list.Count);
        }

        [Test]
        public async Task Get_All_By_DataAsync()
        {
            SetEnvironment();
            var list = await _repository.GetAllByAsync(ExpressionGeneric(), true);
            var result = list.Count;
            Assert.IsNotNull(list);
            Assert.AreEqual(count3, result);
        }

        [Test]
        public async Task Filter_All_DataAsync()
        {
            SetEnvironment();

            var list = await _repository.FilterAllAsync(GetFilter(), true);

            Assert.IsNotNull(list);
            Assert.AreEqual(count3, list.Count);
        }

        [Test]
        public async Task First_DataAsync()
        {
            SetEnvironment();

            var value = await _repository.GetFirstByAsync(ExpressionGeneric(), true);

            Assert.IsNotNull(value);
        }

        [Test]
        public async Task Page_All_DataAsync()
        {
            SetEnvironment();

            var page = await _repository.GetPageAsync(GetPageConfig(), true);

            Assert.IsNotNull(page);
            Assert.AreEqual(count2, page.Content.Count);
        }

        [Test]
        public async Task Page_All_Expression_DataAsync()
        {
            SetEnvironment();

            var page = await _repository.GetPageAsync(GetPageConfig(), ExpressionGeneric(), true);

            Assert.IsNotNull(page);
            Assert.AreEqual(count3, page.Content.Count);
        }

        [Test]
        public async Task Page_All_Filter_DataAsync()
        {
            SetEnvironment();

            var page = await _repository.GetPageAsync(GetPageConfig(), GetFilter(), true);

            Assert.IsNotNull(page);
            Assert.AreEqual(count3, page.Content.Count);
        }

        private void SetEnvironment()
        {
            DeleteBase();
            var data = GetListSimpleObject();
            SaveList(data);
        }

        internal abstract IPageConfig GetPageConfig();
        internal abstract TFilter GetFilter();
        internal abstract Expression<Func<TValue, bool>> ExpressionGeneric();
        internal abstract void SaveList(IEnumerable<TValue> list);
    }
}
