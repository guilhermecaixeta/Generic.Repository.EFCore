using Generic.Repository.Attributes;
using Generic.Repository.UnitTest.Model.Filter;
using NUnit.Framework;

namespace Generic.Repository.UnitTest.Cache
{
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