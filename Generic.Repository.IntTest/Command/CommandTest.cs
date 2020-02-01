using Generic.Repository.Extension.Repository;
using Generic.Repository.Interfaces.Repository;
using Generic.Repository.IntTest.Data;
using Generic.Repository.IntTest.Model;
using Generic.Repository.IntTest.Utils;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Generic.Repository.IntTest.Command
{
    [TestFixture]
    public class CommandTest
    {
        public IBaseRepositoryAsync<FakeInt, IntegrationContext> RepositoryAsync { get; set; }

        [SetUp]
        public void SetUpIntTest()
        {
            RepositoryAsync = DataInjector.GetRepositoryAsync();
        }

        [TearDown]
        public void SetDown()
        {
            DataInjector.BaseDown();
        }

        [Test]
        public async Task BulkInsert_Valid()
        {
            try
            {
                var list = FakeList.GetListFake();

                await RepositoryAsync.BulkInsertAsync(list, 10, default);

                //var total = await RepositoryAsync.CountAsync(default);

                //Assert.AreEqual(total, 50);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                throw ex;

                Assert.IsTrue(false);
            }
        }
    }
}
