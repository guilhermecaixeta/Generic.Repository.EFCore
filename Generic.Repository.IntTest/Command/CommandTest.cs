using Generic.Repository.Extension.Repository;
using Generic.Repository.Interfaces.Repository;
using Generic.RepositoryTest.Int.Data;
using Generic.RepositoryTest.Int.Model;
using Generic.RepositoryTest.Int.Utils;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.RepositoryTest.Int.Command
{
    //TODO: add docker support on cake build to do this tests runs.
    //[Ignore("Need add docker support to this test.")]
    [TestFixture]
    public class CommandTest
    {
        /// <summary>
        /// The chunck size
        /// </summary>
        private const int _chunckSize = 100;

        /// <summary>
        /// The token default
        /// </summary>
        private readonly CancellationToken _tokenDefault = default;

        /// <summary>
        /// Gets or sets the repository asynchronous.
        /// </summary>
        /// <value>
        /// The repository asynchronous.
        /// </value>
        private IBaseRepositoryAsync<FakeInt, IntegrationContext> RepositoryAsync { get; set; }

        /// <summary>
        /// Bulks the insert success.
        /// </summary>
        [Test]
        public async Task BulkInsert_Success()
        {
            var list = FakeFactory.GetListFake();

            await RepositoryAsync.
                BulkInsertAsync(list, _chunckSize, default).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Bulks the delete success.
        /// </summary>
        [Test]
        public async Task BulkDelete_Success()
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

        /// <summary>
        /// Bulks the update success.
        /// </summary>
        [Test]
        public async Task BulkUpdate_Success()
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

        /// <summary>
        /// Creates the transaction asynchronous success.
        /// </summary>
        [Test]
        public async Task CreateTransactionAsync_Success()
        {
            var fakeValue = FakeFactory.GetFake();

            await RepositoryAsync.UnitOfWorkTransactionsAsync(
                async (ctx, token) =>
                {
                    await RepositoryAsync.CreateAsync(fakeValue, token)
                        .ConfigureAwait(false);

                    fakeValue = FakeFactory.UpdateFake(fakeValue);

                    await RepositoryAsync.UpdateAsync(fakeValue, token)
                        .ConfigureAwait(false);

                    await RepositoryAsync.DeleteAsync(fakeValue, token)
                        .ConfigureAwait(false);
                }, _tokenDefault);
        }

        /// <summary>
        /// Inserts the data unit of work success.
        /// </summary>
        [Test]
        public async Task UnitOfWork_InsertData_Success()
        {
            var fake = FakeFactory.GetFake();

            await RepositoryAsync.
                DisableAutotransactionAndBeginTransaction(_tokenDefault).
                ConfigureAwait(false);

            await RepositoryAsync.
                CreateAsync(fake, _tokenDefault, true).
                ConfigureAwait(false);

            await RepositoryAsync.
                SaveChangesAsync(_tokenDefault).
                ConfigureAwait(false);

            await RepositoryAsync.
                CommitAsync(_tokenDefault).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Units the of work update data success.
        /// </summary>
        [Test]
        public async Task UnitOfWork_UpdateData_Success()
        {
            var fake = FakeFactory.GetFake();

            fake = await RepositoryAsync.CreateAsync(fake, _tokenDefault);

            fake = FakeFactory.UpdateFake(fake);

            await RepositoryAsync.
                DisableAutotransactionAndBeginTransaction(_tokenDefault).
                ConfigureAwait(false);

            await RepositoryAsync.UpdateAsync(fake, _tokenDefault, true).
                ConfigureAwait(false);

            await RepositoryAsync.
                SaveChangesAsync(_tokenDefault).
                ConfigureAwait(false);

            await RepositoryAsync.
                CommitAsync(_tokenDefault).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Units the of work delete data success.
        /// </summary>
        [Test]
        public async Task UnitOfWork_DeleteData_Success()
        {
            var fake = FakeFactory.GetFake();

            fake = await RepositoryAsync.CreateAsync(fake, _tokenDefault);

            await RepositoryAsync.
                DisableAutotransactionAndBeginTransaction(_tokenDefault).
                ConfigureAwait(false);

            await RepositoryAsync.
                DeleteAsync(fake, _tokenDefault, true).
                ConfigureAwait(false);

            await RepositoryAsync.
                SaveChangesAsync(_tokenDefault).
                ConfigureAwait(false);

            await RepositoryAsync.
                CommitAsync(_tokenDefault).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Units the of work save changes and commit success.
        /// </summary>
        [Test]
        public async Task UnitOfWork_SaveChangesAndCommit_Success()
        {
            var fake = FakeFactory.GetFake();

            await RepositoryAsync.
                DisableAutotransactionAndBeginTransaction(_tokenDefault).
                ConfigureAwait(false);

            await RepositoryAsync.
                CreateAsync(fake, _tokenDefault, true).
                ConfigureAwait(false);

            await RepositoryAsync.
                SaveChangesAndCommitAsync(_tokenDefault).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Sets down.
        /// </summary>
        [TearDown]
        public void SetDown()
        {
            DataInjector.BaseDown();
        }

        /// <summary>
        /// Sets up int test.
        /// </summary>
        [SetUp]
        public void SetUpIntTest()
        {
            DataInjector.EnsureCreateAndMigrateBase();

            RepositoryAsync = DataInjector.GetRepositoryAsync();
        }
    }
}
