namespace Generic.Repository.Test.Cache
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public abstract class CacheRepositoryExceptionTest<T>
        : CacheRepositoryInsertTest<T>
        where T : class
    {
        private readonly string _emptyValue = string.Empty;

        private readonly string _nullValue = null;

        [Test]
        public async Task GetDictionaryMethodSet_NullValueException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () => await Cache.GetDictionaryMethodSet(_nullValue)));

        [Test]
        public async Task GetDictionaryMethod_EmptyValueException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () => await Cache.GetDictionaryMethodGet(_emptyValue)));

        [Test]
        public async Task GetMethodSet_NullValueException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () => await Cache.GetMethodSet(_nullValue, NameProperty)));

        [Test]
        public async Task GetMethodGet_EmptyValueException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () => await Cache.GetMethodGet(NameType, _emptyValue)));

        [Test]
        public async Task GetDictionaryAttributes_PropertyKeyNotFoundException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () => await Cache.GetMethodGet(NameType, SomeKey)));

        [Test]
        public async Task GetDictionaryAttribute_ObjectKeyNotFoundException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () => await Cache.GetMethodGet(SomeKey, NameProperty)));
    }
}
