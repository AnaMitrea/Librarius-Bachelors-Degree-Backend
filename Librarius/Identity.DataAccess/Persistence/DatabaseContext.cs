using System.Reflection;
using Identity.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    public DbSet<Account> Accounts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Concurrency Problems Example
        // modelBuilder.Entity<...>().UseXminAsConcurrencyToken();

        base.OnModelCreating(modelBuilder);
    }
}