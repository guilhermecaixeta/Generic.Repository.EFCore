using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Generic.Repository.Test.Data
{
    public class DbInMemoryContext<T> : DbContext
    where T : class
    {
        public DbInMemoryContext()
        { }

        public DbInMemoryContext(DbContextOptions<DbInMemoryContext<T>> options)
            : base(options)
        { }

        private DbSet<T> Value { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.
                ConfigureWarnings(warn => warn.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            base.OnConfiguring(optionsBuilder);
        }
    }
}