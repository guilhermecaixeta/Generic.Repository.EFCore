using Generic.Repository.Attributes;
using Generic.Repository.Cache;
using NUnit.Framework;

namespace Generic.Repository.Test.Cache
{
    public class SimpleObject
    {
        [Lambda(MethodOption= Enums.LambdaMethod.Equals)]
        public int Id { get; set; }
    }

    [TestFixture]
    public class CacheRepositoryTest : CacheRepositoryExceptionTest<SimpleObject>
    {
        public CacheRepositoryTest()
        {
            cache = new CacheRepository();
            nameProperty="Id";
            nameAttribute="MethodOption";
        }
    }
}
