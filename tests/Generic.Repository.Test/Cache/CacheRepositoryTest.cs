namespace Generic.Repository.Test.Cache
{
    using Generic.Repository.Attributes;
    using Generic.Repository.Test.Model.Filter;
    using NUnit.Framework;

    [TestFixture]
    public class CacheRepositoryTest : CacheRepositoryExceptionTest<FakeFilter>
    {

        [SetUp]
        public void CacheSetUp()
        {
            NameProperty = nameof(FakeFilter.Value);
            NameAttribute = nameof(FilterAttribute.MethodOption);
        }
    }
}