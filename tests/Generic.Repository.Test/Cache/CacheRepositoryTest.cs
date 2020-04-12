using Generic.Repository.Attributes;
using Generic.RepositoryTest.Unit.Model.Filter;
using NUnit.Framework;

namespace Generic.RepositoryTest.Unit.Cache
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