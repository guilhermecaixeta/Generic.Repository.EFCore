namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Filter;
    using NUnit.Framework;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    [TestFixture]
    public abstract class BaseRepositoryAsyncCommandTest<TValue, TFilter> : BaseRepositoryConfigTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {

        [Test]
        public async Task CreateValueAndGetSingleAsync_ValidValue()
        {
            var value = CreateFakeValue();
            value = await Repository.CreateAsync(value).
                ConfigureAwait(false);

            var result = await Repository.
                GetSingleByAsync(GetFakeExpression(value), false).
                ConfigureAwait(false);

            Assert.AreEqual(value, result);
        }

        [Test]
        public async Task UpdateValueAndGetFirstAsync__ValidValue()
        {
            var value = CreateFakeValue();

            value = await Repository.CreateAsync(value).
                ConfigureAwait(false);

            var valueOutdated = await Repository.
                GetFirstByAsync(GetFakeExpression(value), true).
                ConfigureAwait(false);

            await Repository.UpdateAsync(UpdateFakeValue(value)).
                ConfigureAwait(false);

            Assert.AreNotEqual(value, valueOutdated);
        }

        [Test]
        public async Task DeleteValueAsync__ValidValue()
        {
            var value = CreateFakeValue();
            value = await Repository.CreateAsync(value).
                ConfigureAwait(false);

            await Repository.DeleteAsync(value).
                ConfigureAwait(false);

            var result = await Repository.
                GetFirstByAsync(GetFakeExpression(value), false).
                ConfigureAwait(false);

            Assert.AreEqual(null, result);
        }

        protected abstract TValue CreateFakeValue();
        protected abstract TValue UpdateFakeValue(TValue value);
        protected abstract Expression<Func<TValue, bool>> GetFakeExpression(TValue value);
    }
}
