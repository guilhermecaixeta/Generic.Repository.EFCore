using Microsoft.EntityFrameworkCore;

namespace Generic.Repository.Test.Data
{
    public class DbInMemoryContext<T> : DbContext
    where T : class
    {
        DbSet<T> Value { get; set; }

        public DbInMemoryContext()
        { }

        public DbInMemoryContext(DbContextOptions<DbInMemoryContext<T>> options)
            : base(options)
        { }

    }
}
