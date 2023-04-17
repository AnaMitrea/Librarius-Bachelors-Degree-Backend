using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id");
        
        builder.Property(x => x.BookshelfId)
            .HasColumnName("bookshelf_id")
            .IsRequired();
        
        builder.Property(x => x.Title)
            .HasColumnName("category_title")
            .IsRequired();
        

        builder.Property(x => x.Link)
            .HasColumnName("link");
        
        // 1 bookshelf has * categories, bookshelfId as Foreign Key
        builder.Property(x => x.BookshelfId).IsRequired();
        builder.HasOne(category => category.Bookshelf)
            .WithMany(bookshelf => bookshelf.Categories)
            .HasForeignKey(category => category.BookshelfId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}