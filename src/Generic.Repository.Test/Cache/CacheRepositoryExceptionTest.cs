namespace Generic.Repository.Test.Cache
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public abstract class CacheRepositoryExceptionTest<T>
        : CacheRepositoryInsertTest<T>
        where T: class
    {
        private readonly string _emptyvalue = string.Empty;
        private const string Fakekey = "ABCDE";

        [Test]
        public void Get_Exception_Dictionary_Method_Set()
        {
            InitCache();
           Assert.Throws<ArgumentNullException>(() => cache.GetDictionaryMethodSet(null));
        }

        [Test]
        public void Get_Exception_Dictionary_Method_Get()
        {
            Assert.Throws<ArgumentNullException>(() => Cache.GetDictionaryMethodGet(_emptyvalue));
        }

        [Test]
        public void Get_Exception_Method_Set()
        {
            InitCache();
            Assert.Throws<ArgumentNullException>(() => cache.GetMethodSet(null, nameProperty));
        }

        [Test]
        public void Get_Exception_Method_Get()
        {
<<<<<<< Updated upstream:src/Generic.Repository.Test/Cache/CacheRepositoryExceptionTest.cs
            InitCache();
            Assert.Throws<ArgumentNullException>(() => cache.GetMethodGet(typeof(T).Name, ""));
=======
            Assert.Throws<ArgumentNullException>(() => Cache.GetMethodGet(Typename, _emptyvalue));
>>>>>>> Stashed changes:tests/Generic.Repository.Test/Cache/CacheRepositoryExceptionTest.cs
        }

        [Test]
        public void Get_Exception_Dictionary_Attributes()
        {
<<<<<<< Updated upstream:src/Generic.Repository.Test/Cache/CacheRepositoryExceptionTest.cs
            InitCache();
            Assert.Throws<KeyNotFoundException>(() => cache.GetMethodGet(typeof(T).Name, "ABDC"));
=======
            Assert.Throws<KeyNotFoundException>(() => Cache.GetMethodGet(Typename, Fakekey));
>>>>>>> Stashed changes:tests/Generic.Repository.Test/Cache/CacheRepositoryExceptionTest.cs
        }

        [Test]
        public void Get_Exception_Dictionary_Attribute()
        {
<<<<<<< Updated upstream:src/Generic.Repository.Test/Cache/CacheRepositoryExceptionTest.cs
            InitCache();
            Assert.Throws<KeyNotFoundException>(() => cache.GetMethodGet("ABCDE", nameProperty));
=======
            Assert.Throws<KeyNotFoundException>(() => Cache.GetMethodGet(Fakekey, NameProperty));
>>>>>>> Stashed changes:tests/Generic.Repository.Test/Cache/CacheRepositoryExceptionTest.cs
        }
    }
}
