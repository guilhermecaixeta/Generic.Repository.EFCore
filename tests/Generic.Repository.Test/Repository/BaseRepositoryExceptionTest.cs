namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Filter;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public abstract class BaseRepositoryExceptionTest<TValue, TFilter> : BaseRepositoryAsyncQueryTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        private readonly TValue _value;

        [Test]
        public void Create_NullValue() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.CreateAsync(_value);
            });

        [Test]
        public void Update_NullValue() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.UpdateAsync(_value);
            });

        [Test]
        public void Delete_NullValue() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.DeleteAsync(_value);
            });

        [Test]
        public void GetPage_NullConfig() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetPageAsync(null, true);
            });

        [Test]
        public void GetPage_NullPredicate() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                var expression = GetFakeExpression();
                expression = null;
                await Repository.GetPageAsync(GetPageConfigFake(), expression, true);
            });

        [Test]
        public void GetPage_NullFilter() =>
            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                var filter = GetFilterFake();
                filter = null;
                await Repository.GetPageAsync(GetPageConfigFake(), filter, true);
            });

        [Test]
        public void GetFirstAsync_NullPredicate() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetFirstByAsync(null, true);
            });

        [Test]
        public void GetSingleAsync_NullPredicate() =>
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await Repository.GetSingleByAsync(null, true);
            });
    }
}
