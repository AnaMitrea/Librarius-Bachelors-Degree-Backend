using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Trophy.DataAccess.Entities;

namespace Trophy.DataAccess.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Entities.Trophy> Trophies { get; set; }
    
    public DbSet<TrophyUserReward> TrophyAccounts { get; set; }
    
    public DbSet<TrophyUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Concurrency Problems Example
        // modelBuilder.Entity<...>().UseXminAsConcurrencyToken();

        base.OnModelCreating(modelBuilder);
    }
}