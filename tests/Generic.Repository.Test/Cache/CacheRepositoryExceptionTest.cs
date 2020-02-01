using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Repository.UnitTest.Cache
{
    [TestFixture]
    public abstract class CacheRepositoryExceptionTest<T>
        : CacheRepositoryGetSetValuesTest<T>
        where T : class
    {
        private readonly string _emptyValue = string.Empty;

        private readonly string _nullValue = null;

        [Test]
        public async Task GetDictionaryMethod_EmptyValueException() =>
            await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await Cache.GetDictionaryMethodGet(_emptyValue, default).ConfigureAwait(false))).
                ConfigureAwait(false);

        [Test]
        public async Task GetDictionaryMethodSet_NullValueException() =>
            await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await Cache.GetDictionaryMethodSet(_nullValue, default).
                    ConfigureAwait(false))).
                ConfigureAwait(false);

        [Test]
        public async Task GetMethodGet_AttributeNoCacheable() =>
            await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await Cache.GetMethodGet(SomeKey, NoCacheableProperty, default).
                    ConfigureAwait(false))).
                ConfigureAwait(false);

        [Test]
        public async Task GetMethodGet_EmptyValueException() =>
            await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await Cache.GetMethodGet(NameType, _emptyValue, default).
                    ConfigureAwait(false))).
                ConfigureAwait(false);

        [Test]
        public async Task GetMethodGet_ObjectKeyNotFoundException() =>
            await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await Cache.GetMethodGet(SomeKey, NameProperty, default).
                    ConfigureAwait(false))).
                ConfigureAwait(false);

        [Test]
        public async Task GetMethodGet_PropertyKeyNotFoundException() =>
            await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await Cache.GetMethodGet(NameType, SomeKey, default).
                    ConfigureAwait(false)))
                .ConfigureAwait(false);

        [Test]
        public async Task GetMethodSet_AttributeNoCacheable() =>
            await Task.FromResult(Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await Cache.GetMethodSet(SomeKey, NoCacheableProperty, default).
                    ConfigureAwait(false))).
                ConfigureAwait(false);

        [Test]
        public async Task GetMethodSet_NullValueException() =>
            await Task.FromResult(Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await Cache.GetMethodSet(_nullValue, NameProperty, default).
                    ConfigureAwait(false))).
                ConfigureAwait(false);
    }
}