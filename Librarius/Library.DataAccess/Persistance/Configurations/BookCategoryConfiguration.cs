using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistance.Configurations;

public class BookCategoryConfiguration : IEntityTypeConfiguration<Entities.BookCategory>
{
    public void Configure(EntityTypeBuilder<Entities.BookCategory> builder)
    {
        builder.ToTable("booksCategories");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id");
        
        builder.Property(x => x.BookId)
            .HasColumnName("book_id")
            .IsRequired();
        builder.Property(x => x.CategoryId)
            .HasColumnName("category_id")
            .IsRequired();

        // many-to-many for books and categories
        
        builder
            .HasOne(bookCateg => bookCateg.Book)
            .WithMany(book => book.BookCategories)
            .HasForeignKey(bookCateg => bookCateg.BookId);
        
        builder
            .HasOne(bookCateg => bookCateg.Category)
            .WithMany(category => category.BookCategories)
            .HasForeignKey(bookCateg => bookCateg.CategoryId);
    }
}