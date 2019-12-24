using Microsoft.EntityFrameworkCore;

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
    }
}