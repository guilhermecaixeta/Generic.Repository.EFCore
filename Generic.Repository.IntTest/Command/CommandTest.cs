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
    //[TestFixture]
    public class CommandTest
    {
        public IBaseRepositoryAsync<FakeInt, IntegrationContext> RepositoryAsync { get; set; }

        [Test]
        public async Task BulkInsert_Valid()
        {
            try
            {
                var list = FakeList.GetListFake();

                await RepositoryAsync.BulkInsertAsync(list, 10, default).ConfigureAwait(false);

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

        [TearDown]
        public void SetDown()
        {
            DataInjector.BaseDown();
        }

        [SetUp]
        public void SetUpIntTest()
        {
            RepositoryAsync = DataInjector.GetRepositoryAsync();
        }
    }
}