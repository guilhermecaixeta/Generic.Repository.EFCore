using Generic.Repository.Models.Filter;
using Generic.Repository.Models.Page.PageConfig;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Generic.Repository.Test.Repository
{
    [TestFixture]
    public abstract class BaseRepositoryAsyncQueryTest<TValue, TFilter>
        : BaseRespositoryExceptionTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        protected int count;
        protected int count2;
        protected int count3;

        public BaseRepositoryAsyncQueryTest()
        {
            DeleteBase();
        }

        protected abstract IEnumerable<TValue> GetListSimpleObject();

        [Test]
        public async Task Get_All_DataAsync()
        {
            var data = GetListSimpleObject();
            SaveList(data);
            var list = await _repository.GetAllAsync(true);

            Assert.IsNotNull(list);
            Assert.AreEqual(count, list.Count);
            DeleteBase();
        }

        [Test]
        public async Task Get_All_By_DataAsync()
        {
            var data = GetListSimpleObject();
            SaveList(data);
            var list = await _repository.GetAllByAsync(ExpressionGeneric() ,true);
            var result = list.Count;
            Assert.IsNotNull(list);
            Assert.AreEqual(count2, result);
            DeleteBase();
        }

        [Test]
        public async Task Filter_All_DataAsync()
        {
            var data = GetListSimpleObject();
            SaveList(data);
            var list = await _repository.FilterAllAsync(GetFilter(), true);

            Assert.IsNotNull(list);
            Assert.AreEqual(count3, list.Count);
            DeleteBase();
        }

        [Test]
        public async Task First_DataAsync()
        {
            var data = GetListSimpleObject();
            SaveList(data);
            var value = await _repository.GetFirstByAsync(ExpressionGeneric(), true);

            Assert.IsNotNull(value);
            DeleteBase();
        }

        [Test]
        public async Task Page_All_DataAsync()
        {
            var data = GetListSimpleObject();
            SaveList(data);
            var page = await _repository.GetPageAsync(GetPageConfig(), true);

            Assert.IsNotNull(page);
            Assert.AreEqual(page.TotalElements, count2);
            DeleteBase();
        }

        [Test]
        public async Task Page_All_Expression_DataAsync()
        {
            var data = GetListSimpleObject();
            SaveList(data);
            var page = await _repository.GetPageAsync(GetPageConfig(), ExpressionGeneric(), true);

            Assert.IsNotNull(page);
            Assert.AreEqual(page.TotalElements, count2);
            DeleteBase();
        }

        [Test]
        public async Task Page_All_Filter_DataAsync()
        {
            var data = GetListSimpleObject();
            SaveList(data);
            var page = await _repository.GetPageAsync(GetPageConfig(), GetFilter(), true);

            Assert.IsNotNull(page);
            Assert.AreEqual(page.TotalElements, count3);
            DeleteBase();
        }

        internal abstract IPageConfig GetPageConfig();
        internal abstract TFilter GetFilter();
        internal abstract Expression<Func<TValue, bool>> ExpressionGeneric();
        internal abstract void SaveList(IEnumerable<TValue> list);
    }
}
