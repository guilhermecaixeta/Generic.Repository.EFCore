using Generic.Repository.Cache;
using Generic.Repository.Interfaces.Repository;
using Generic.RepositoryTest.Int.Model;
using Generic.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace Generic.RepositoryTest.Int.Data
{
    public static class DataInjector
    {
        /// <summary>
        /// Bases down.
        /// </summary>
        public static void BaseDown()
        {

            using (var ctx = CreateAndGetContext())
            {
                ctx.Database.EnsureDeleted();

                // TODO: Remove this after add docker support on cake.
                //var list = ctx.FakeInt.AsNoTracking();
                //ctx.FakeInt.RemoveRange(list);
                //ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Creates the and get context.
        /// </summary>
        /// <returns></returns>
        public static IntegrationContext CreateAndGetContext()
        {
            var connectionString = "host=localhost;port=5432;Username=postgres;Password=123456;Database=Test";

            var builder = new DbContextOptionsBuilder<IntegrationContext>();

            builder.UseNpgsql(connectionString, opt =>
            {
                opt.EnableRetryOnFailure();
                opt.MaxBatchSize(100);
            });

            var context = new IntegrationContext(builder.Options);

            return context;
        }

        /// <summary>
        /// Ensures the create and migrate base.
        /// </summary>
        public static void EnsureCreateAndMigrateBase()
        {
            using (var context = CreateAndGetContext())
            {
                context.Database.EnsureCreated();

                context.Database.Migrate();
            }
        }

        /// <summary>
        /// Gets the repository asynchronous.
        /// </summary>
        /// <returns></returns>
        public static IBaseRepositoryAsync<FakeInt, IntegrationContext> GetRepositoryAsync()
        {
            var cache = new CacheRepository();

            var context = CreateAndGetContext();

            return new BaseRepositoryAsync<FakeInt, IntegrationContext>(context, cache);
        }
    }
}