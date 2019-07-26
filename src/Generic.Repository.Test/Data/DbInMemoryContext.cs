using System;
using Microsoft.EntityFrameworkCore;

namespace Generic.Repository.Test.Data
{
    public class DbInMemoryContext<T> : DbContext
    where T : class
    {
        DbSet<T> value {get; set;}
        public DbInMemoryContext(DbContextOptions options) : base(options){}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseInMemoryDatabase("DBInMemory");
        
    }
}
