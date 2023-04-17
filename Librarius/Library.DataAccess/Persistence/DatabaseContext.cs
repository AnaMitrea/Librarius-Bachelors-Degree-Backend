using System.Reflection;
using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    public DbSet<Bookshelf> Bookshelves { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }
    
    public DbSet<BookCategory> BooksCategories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Concurrency Problems Example
        // modelBuilder.Entity<...>().UseXminAsConcurrencyToken();

        base.OnModelCreating(modelBuilder);
    }
}