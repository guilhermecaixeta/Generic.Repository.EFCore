namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Filter;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    [TestFixture]
    public abstract class BaseRepositoryAsyncCommandTest<TValue, TFilter> : BaseRepositoryConfigTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        internal abstract IEnumerable<TValue> GetListFake();

        [Test]
        public async Task CreateValueAsync_ValidValue()
        {
            var value = CreateFakeValue();
            value = await Repository.
                CreateAsync(value).
                ConfigureAwait(false);

            Assert.NotNull(value);
        }

        [Test]
        public async Task UpdateValueAndGetFirstAsync_ValidValue()
        {
            var value = CreateFakeValue();

            value = await Repository.
                CreateAsync(value).
                ConfigureAwait(false);

            var valueOutdated = await Repository.
                GetFirstByAsync(GetFakeExpression(value), true).
                ConfigureAwait(false);

            await Repository.
                UpdateAsync(UpdateFakeValue(value)).
                ConfigureAwait(false);

            Assert.AreNotEqual(value, valueOutdated);
        }

        [Test]
        public async Task DeleteValueAsync_ValidValue()
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

        [Test]
        public async Task CreateListAsync_ValidValue()
        {
            BaseTearDown();
            var list = GetListFake();
            await Repository.
                CreateAsync(list).
                ConfigureAwait(false);

            var count = await Repository.
                CountAsync().
                ConfigureAwait(false);

            Assert.AreEqual(list.Count(), count);
        }

        [Test]
        public async Task UpdateListAsync_ValidValue()
        {

            var listOutdated = await Repository.
                GetAllAsync(false).
                ConfigureAwait(false);

            var listUpdated = await Repository.
                GetAllAsync(false).
                ConfigureAwait(false);

            listUpdated.
                ToList().
                ForEach(x => UpdateFakeValue(x));

            await Repository.
                UpdateAsync(listUpdated).
                ConfigureAwait(false);

            var result = listOutdated.Equals(listUpdated);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteListAsync_ValidValue()
        {
            var list = await Repository.
                GetAllAsync(false).
                ConfigureAwait(false);

            await Repository.DeleteAsync(list).
                ConfigureAwait(false);

            var count = await Repository.
                CountAsync().
                ConfigureAwait(false);

            Assert.AreEqual(0, count);
        }

        protected abstract TValue CreateFakeValue();
        protected abstract TValue UpdateFakeValue(TValue value);
        protected abstract Expression<Func<TValue, bool>> GetFakeExpression(TValue value);
    }
}
