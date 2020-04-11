using Generic.Repository.Extension.Repository;
using Generic.Repository.Interfaces.Repository;
using Generic.Repository.IntTest.Data;
using Generic.Repository.IntTest.Model;
using Generic.Repository.IntTest.Utils;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generic.Repository.IntTest.Command
{
    //TODO: add docker support on cake build to do this tests runs.
    [Ignore("Need add docker support to this test.")]
    [TestFixture]
    public class CommandTest
    {
        private const int _chunckSize = 10;

        public IBaseRepositoryAsync<FakeInt, IntegrationContext> RepositoryAsync { get; set; }

        [Test]
        public async Task BulkInsert_Valid()
        {
            var list = FakeFactory.GetListFake();

            await RepositoryAsync.
                BulkInsertAsync(list, _chunckSize, default).
                ConfigureAwait(false);
        }

        [Test]
        public async Task BulkDelete_Valid()
        {
            await FakeFactory.InsertData();
            using (var context = DataInjector.CreateAndGetContext())
            {
                var list = context.FakeInt.AsQueryable();
                await RepositoryAsync.
                    BulkDeleteAsync(list, _chunckSize, default).
                    ConfigureAwait(false);
            }
        }

        [Test]
        public async Task BulkUpdate_Valid()
        {
            await FakeFactory.InsertData();

            var list = await RepositoryAsync.
                GetAllAsync(true, default).
                ConfigureAwait(false);

            var newestList = FakeFactory.
                    UpdateList(list);

            await RepositoryAsync.
                BulkUpdateAsync(newestList, _chunckSize, default).
                ConfigureAwait(false);

        }

        [Test]
        public async Task CreateTransactionAsync_ValidValue()
        {
            var fakeValue = FakeFactory.GetFake();

            await RepositoryAsync.UnitOfWorkTransactionsAsync(
                async ctx =>
                {
                    await RepositoryAsync.CreateAsync(fakeValue, default)
                        .ConfigureAwait(false);

                    fakeValue = FakeFactory.UpdateFake(fakeValue);

                    await RepositoryAsync.UpdateAsync(fakeValue, default)
                        .ConfigureAwait(false);

                    await RepositoryAsync.DeleteAsync(fakeValue, default)
                        .ConfigureAwait(false);
                }, default);
        }

        [TearDown]
        public void SetDown()
        {
            DataInjector.BaseDown();
        }

        [SetUp]
        public void SetUpIntTest()
        {
            DataInjector.EnsureCreateAndMigrateBase();

            RepositoryAsync = DataInjector.GetRepositoryAsync();
        }
    }
}
