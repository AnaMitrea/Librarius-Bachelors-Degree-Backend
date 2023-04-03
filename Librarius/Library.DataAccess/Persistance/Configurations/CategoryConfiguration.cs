using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistance.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(x => x.Id);
        
        // 1 bookshelf has * categories, bookshelfId as Foreign Key
        builder.Property(x => x.BookshelfId).IsRequired();
        builder.HasOne(category => category.Bookshelf)
            .WithMany(bookshelf => bookshelf.Categories)
            .HasForeignKey(category => category.BookshelfId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}