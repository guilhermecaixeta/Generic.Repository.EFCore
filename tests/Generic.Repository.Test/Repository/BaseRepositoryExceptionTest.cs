using Generic.Repository.Exceptions;
using Generic.Repository.Models.Filter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Generic.RepositoryTest.Unit.Repository
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
        public void Create_NullValue_Exception() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.CreateAsync(_value, default).ConfigureAwait(false);
            });

        [Test]
        public void CreateList_EmptyList_Exception() =>
            Assert.ThrowsAsync<ListNullOrEmptyException>(async () =>
            {
                await Repository.CreateAsync(new List<TValue>(), default).ConfigureAwait(false);
            });

        [Test]
        public void CreateList_NullList_Exception() =>
            Assert.ThrowsAsync<ListNullOrEmptyException>(async () =>
            {
                await Repository.CreateAsync((IEnumerable<TValue>)null, default).ConfigureAwait(false);
            });

        [Test]
        public void Delete_NullValue_Exception() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.DeleteAsync(_value, default).ConfigureAwait(false);
            });

        [Test]
        public void DeleteList_EmptyList_Exception() =>
            Assert.ThrowsAsync<ListNullOrEmptyException>(async () =>
            {
                await Repository.DeleteAsync(new List<TValue>(), default).ConfigureAwait(false);
            });

        [Test]
        public void DeleteList_NullList_Exception() =>
            Assert.ThrowsAsync<ListNullOrEmptyException>(async () =>
            {
                await Repository.DeleteAsync((IEnumerable<TValue>)null, default).ConfigureAwait(false);
            });

        [Test]
        public void GetFirstAsync_NullPredicate_Exception() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetFirstOrDefaultAsync(null, true, default).ConfigureAwait(false);
            });

        [Test]
        public void GetPage_NullConfig_Exception_Exception() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetPageAsync(null, true, default).ConfigureAwait(false);
            });

        [Test]
        public void GetPage_NullFilter_Exception() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetPageAsync(GetPageConfigFake(), null, true, default).ConfigureAwait(false);
            });

        [Test]
        public void GetPage_NullPredicate_Exception() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetPageAsync(GetPageConfigFake(), (Expression<Func<TValue, bool>>)null, true, default).ConfigureAwait(false);
            });

        [Test]
        public void GetSingleAsync_NullPredicate_Exception() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetSingleOrDefaultAsync(null, true, default).ConfigureAwait(false);
            });

        [Test]
        public void Update_NullValue_Exception() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.UpdateAsync(_value, default).ConfigureAwait(false);
            });
    }
}