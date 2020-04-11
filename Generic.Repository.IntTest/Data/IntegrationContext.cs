using Generic.Repository.IntTest.Model;
using Microsoft.EntityFrameworkCore;

namespace Generic.Repository.IntTest.Data
{
    public class IntegrationContext : DbContext
    {
        public IntegrationContext(DbContextOptions<IntegrationContext> options)
               : base(options)
        { }

        public DbSet<FakeInt> FakeInt { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}