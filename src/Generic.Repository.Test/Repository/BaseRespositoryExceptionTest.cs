using Generic.Repository.Models.Filter;
using NUnit.Framework;
using System;

namespace Generic.Repository.Test.Repository
{
    [TestFixture]
    public abstract class BaseRespositoryExceptionTest<TValue, TFilter> : BaseRepositoryAsyncCommandTest<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        private readonly TValue value = null;

        [Test]
        public void Create_Null_Value()
        => Assert.ThrowsAsync<NullReferenceException>(async () => { await _repository.CreateAsync(value); });

        [Test]
        public void Update_Null_Value()
        => Assert.ThrowsAsync<NullReferenceException>(async () => { await _repository.UpdateAsync(value); });

        [Test]
        public void Delete_Null_Value()
        => Assert.ThrowsAsync<NullReferenceException>(async () => { await _repository.DeleteAsync(value); });
    }
}