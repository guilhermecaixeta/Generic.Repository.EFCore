namespace Generic.Repository.Test.Cache
{
    using Generic.Repository.Attributes;
    using Generic.Repository.Test.Model.Filter;
    using NUnit.Framework;

    [TestFixture]
    public class CacheRepositoryTest : CacheRepositoryIOAccessTest<FakeFilter>
    {
        [SetUp]
        public void CacheSetUp()
        {
            NameProperty = nameof(FakeFilter.Value);
            NoCacheableProperty = nameof(FakeFilter.Unkown);
            NameAttribute = nameof(FilterAttribute.MethodOption);
        }
    }
}