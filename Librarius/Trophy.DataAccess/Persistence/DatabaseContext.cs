using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Trophy.DataAccess.Entities;

namespace Trophy.DataAccess.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Entities.Trophy> Trophies { get; set; }
    
    public DbSet<TrophyUserReward> TrophyUserReward { get; set; }
    
    public DbSet<TrophyRewardReadingTime> TrophyRewardReadingTime { get; set; }
    
    public DbSet<TrophyRewardReadingBooks> TrophyRewardReadingBooks { get; set; }
    
    public DbSet<TrophyRewardCategoryReader> TrophyRewardCategoryReader { get; set; }
    
    public DbSet<TrophyRewardActivities> TrophyRewardActivities { get; set; }
    
    public DbSet<Level> Levels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Concurrency Problems Example
        // modelBuilder.Entity<...>().UseXminAsConcurrencyToken();

        base.OnModelCreating(modelBuilder);
    }
}