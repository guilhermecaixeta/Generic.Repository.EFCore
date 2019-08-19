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
        public BaseRepositoryAsyncCommandTest() { }

        [Test]
        public async Task Create_Valid_Value_And_CountAsync()
        {
            var value = CreateFakeValue();
            value = await _repository.CreateAsync(value);
            var result = await _repository.GetSingleByAsync(ExpressionGeneric(value), false);

            Assert.AreEqual(value, result);
        }

        [Test]
        public async Task Update_Valid_Value_And_Get_SingleAsync()
        {
            var value = CreateFakeValue();

            value = await _repository.CreateAsync(value).ConfigureAwait(false);

            var valueOutdated = await _repository.GetFirstByAsync(ExpressionGeneric(value), true).ConfigureAwait(false);

            await _repository.UpdateAsync(UpdateFakeValue(value)).ConfigureAwait(false);

            Assert.AreNotEqual(value, valueOutdated);
        }

        [Test]
        public async Task Delete_ValueAsync()
        {
            var value = CreateFakeValue();
            value = await _repository.CreateAsync(value).ConfigureAwait(false);
            await _repository.DeleteAsync(value).ConfigureAwait(false);

            var result = await _repository.GetFirstByAsync(ExpressionGeneric(value), false).ConfigureAwait(false);

            Assert.AreEqual(null, result);
        }

        protected abstract TValue CreateFakeValue();
        protected abstract TValue UpdateFakeValue(TValue value);
        protected abstract Expression<Func<TValue, bool>> ExpressionGeneric(TValue value);
    }
}
