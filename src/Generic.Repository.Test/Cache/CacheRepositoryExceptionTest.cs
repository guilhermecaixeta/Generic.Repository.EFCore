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

        [Test]
        public void Get_Exception_Dictionary_Method_Set()
        {
            InitCache();
           Assert.Throws<ArgumentNullException>(() => cache.GetDictionaryMethodSet(null));
        }

        [Test]
        public void Get_Exception_Dictionary_Method_Get()
        {
            InitCache();
            Assert.Throws<ArgumentNullException>(() => cache.GetDictionaryMethodGet(""));
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
            InitCache();
            Assert.Throws<ArgumentNullException>(() => cache.GetMethodGet(typeof(T).Name, ""));
        }

        [Test]
        public void Get_Exception_Dictionary_Attributes()
        {
            InitCache();
            Assert.Throws<KeyNotFoundException>(() => cache.GetMethodGet(typeof(T).Name, "ABDC"));
        }

        [Test]
        public void Get_Exception_Dictionary_Attribute()
        {
            InitCache();
            Assert.Throws<KeyNotFoundException>(() => cache.GetMethodGet("ABCDE", nameProperty));
        }
    }
}
