using DoomedDatabases.Postgres;
using Generic.Repository.Cache;
using Generic.Repository.Interfaces.Repository;
using Generic.Repository.IntTest.Model;
using Generic.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace Generic.Repository.IntTest.Data
{
    public static class DataInjector
    {
        private static ITestDatabase testDatabase;

        public static void BaseDown()
        {
            testDatabase.Drop();
        }

        public static IntegrationContext CreateAndGetContext()
        {
            var connectionString = "host=localhost;port=5432;Username=postgres;Password=048365;Database=Test";

            testDatabase = new TestDatabaseBuilder().WithConnectionString(connectionString).Build();

            testDatabase.Create();

            var builder = new DbContextOptionsBuilder<IntegrationContext>();

            builder.UseNpgsql(testDatabase.ConnectionString);

            var context = new IntegrationContext(builder.Options);

            context.Database.EnsureCreated();

            return context;
        }

        public static IBaseRepositoryAsync<FakeInt, IntegrationContext> GetRepositoryAsync()
        {
            var cache = new CacheRepository();

            var injectorCtx = CreateAndGetContext();

            injectorCtx.Database.Migrate();

            return new BaseRepositoryAsync<FakeInt, IntegrationContext>(injectorCtx, cache);
        }
    }
}