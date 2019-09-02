namespace Generic.Repository.Test.Cache
{
    using Generic.Repository.Attributes;
    using NUnit.Framework;

    public class FakeObject
    {
        [Lambda(MethodOption = Enums.LambdaMethod.Equals)]
        public int Id { get; set; }
    }

    [TestFixture]
    public class CacheRepositoryTest : CacheRepositoryExceptionTest<FakeObject>
    {

        [SetUp]
        public void CacheSetUp()
        {
            NameProperty = nameof(FakeObject.Id);
            NameAttribute = nameof(LambdaAttribute.MethodOption);
        }
    }
}
