using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Generic.Repository.Exceptions;
using Generic.Repository.Models.Filter;
using NUnit.Framework;

namespace Generic.Repository.UnitTest.Repository
{
    [TestFixture]
    public abstract class BaseRepositoryExceptionTest<TValue, TFilter> : BaseRepositoryAsyncQueryTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter

    {
        private readonly TValue _value;

        protected BaseRepositoryExceptionTest()
        {
            _value = null;
        }

        [Test]
        public void Create_NullValue() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.CreateAsync(_value, default);
            });

        [Test]
        public void CreateList_EmptyList() =>
            Assert.ThrowsAsync<ListNullOrEmptyException>(async () =>
            {
                await Repository.CreateAsync(new List<TValue>(), default);
            });

        [Test]
        public void CreateList_NullList() =>
            Assert.ThrowsAsync<ListNullOrEmptyException>(async () =>
            {
                await Repository.CreateAsync((IEnumerable<TValue>)null, default);
            });

        [Test]
        public void Delete_NullValue() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.DeleteAsync(_value, default);
            });

        [Test]
        public void DeleteList_EmptyList() =>
            Assert.ThrowsAsync<ListNullOrEmptyException>(async () =>
            {
                await Repository.DeleteAsync(new List<TValue>(), default);
            });

        [Test]
        public void DeleteList_NullList() =>
            Assert.ThrowsAsync<ListNullOrEmptyException>(async () =>
            {
                await Repository.DeleteAsync((IEnumerable<TValue>)null, default);
            });

        [Test]
        public void GetFirstAsync_NullPredicate() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetFirstByAsync(null, true, default);
            });

        [Test]
        public void GetPage_NullConfig() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetPageAsync(null, true, default);
            });

        [Test]
        public void GetPage_NullFilter() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetPageAsync(GetPageConfigFake(), null, true, default);
            });

        [Test]
        public void GetPage_NullPredicate() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetPageAsync(GetPageConfigFake(), (Expression<Func<TValue, bool>>)null, true, default);
            });

        [Test]
        public void GetSingleAsync_NullPredicate() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetSingleByAsync(null, true, default);
            });

        [Test]
        public void Update_NullValue() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.UpdateAsync(_value, default);
            });
    }
}