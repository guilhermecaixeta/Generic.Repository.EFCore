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
            _ = await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () => await Cache.GetDictionaryMethodSet(_nullValue, default)));

        [Test]
        public async Task GetDictionaryMethod_EmptyValueException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () => await Cache.GetDictionaryMethodGet(_emptyValue, default)));

        [Test]
        public async Task GetMethodSet_NullValueException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () => await Cache.GetMethodSet(_nullValue, NameProperty, default)));

        [Test]
        public async Task GetMethodGet_EmptyValueException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () => await Cache.GetMethodGet(NameType, _emptyValue, default)));

        [Test]
        public async Task GetMethodGet_PropertyKeyNotFoundException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () => await Cache.GetMethodGet(NameType, SomeKey, default)));

        [Test]
        public async Task GetMethodGet_ObjectKeyNotFoundException() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () => await Cache.GetMethodGet(SomeKey, NameProperty, default)));

        [Test]
        public async Task GetMethodGet_ObjectNoCacheable() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () => await Cache.GetMethodGet(SomeKey, NoCacheableProperty, default)));

        [Test]
        public async Task GetMethodSet_ObjectNoCacheable() =>
            _ = await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () => await Cache.GetMethodSet(SomeKey, NoCacheableProperty, default)));

    }
}
