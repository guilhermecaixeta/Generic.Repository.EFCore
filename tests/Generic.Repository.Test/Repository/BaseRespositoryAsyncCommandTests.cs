using Generic.Repository.Models.Filter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Generic.Repository.UnitTest.Repository
{
    [TestFixture]
    public abstract class BaseRepositoryAsyncCommandTest<TValue, TFilter>
        : BaseRepositoryConfigTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        [Test]
        public async Task CreateListAsync_ValidValue()
        {
            var list = GetListFake();

            await Repository.
                CreateAsync(list, default).
                ConfigureAwait(false);

            var count = await Repository.
                CountAsync(default).
                ConfigureAwait(false);

            Assert.AreEqual(100, count);
        }

        [Test]
        public async Task CreateValueAsync_ValidValue()
        {
            var value = CreateFakeValue();
            value = await Repository.
                CreateAsync(value, default).
                ConfigureAwait(false);

            Assert.NotNull(value);
        }

        [Test]
        public async Task DeleteListAsync_ValidValue()
        {
            var list = await Repository.
                GetAllAsync(false, default).
                ConfigureAwait(false);

            await Repository.DeleteAsync(list, default).
                ConfigureAwait(false);

            var count = await Repository.
                CountAsync(default).
                ConfigureAwait(false);

            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task DeleteValueAsync_ValidValue()
        {
            var value = CreateFakeValue();
            value = await Repository.CreateAsync(value, default).
                ConfigureAwait(false);

            await Repository.DeleteAsync(value, default).
                ConfigureAwait(false);

            var result = await Repository.
                GetFirstOrDefaultAsync(GetFakeExpression(value), false, default).
                ConfigureAwait(false);

            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task UpdateListAsync_ValidValue()
        {
            var listOutdated = await Repository.
                GetAllAsync(false, default).
                ConfigureAwait(false);

            var listUpdated = await Repository.
                GetAllAsync(false, default).
                ConfigureAwait(false);

            listUpdated.
                ToList().
                ForEach(x => UpdateFakeValue(x));

            await Repository.
                UpdateAsync(listUpdated, default).
                ConfigureAwait(false);

            var result = listOutdated.Equals(listUpdated);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateValueAndGetFirstAsync_ValidValue()
        {
            var value = CreateFakeValue();

            value = await Repository.
                CreateAsync(value, default).
                ConfigureAwait(false);

            var valueOutdated = await Repository.
                GetFirstOrDefaultAsync(GetFakeExpression(value), true, default).
                ConfigureAwait(false);

            await Repository.
                UpdateAsync(UpdateFakeValue(value), default).
                ConfigureAwait(false);

            Assert.AreNotEqual(value, valueOutdated);
        }

        internal abstract IEnumerable<TValue> GetListFake();

        protected abstract TValue CreateFakeValue();

        protected abstract Expression<Func<TValue, bool>> GetFakeExpression(TValue value);

        protected abstract TValue UpdateFakeValue(TValue value);
    }
}