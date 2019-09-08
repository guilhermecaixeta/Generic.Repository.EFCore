namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Filter;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public abstract class BaseRepositoryExceptionTest<TValue, TFilter> : BaseRepositoryAsyncCommandTest<TValue, TFilter>
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
    }
}
