using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");
        
        builder.Property(x => x.Id)
            .HasColumnName("id");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired();
        
        builder.Property(x => x.Author)
            .HasColumnName("author")
            .IsRequired();

        builder.Property(x => x.Link)
            .HasColumnName("link");
        
        builder.Property(x => x.Language)
            .HasColumnName("language");
        
        builder.Property(x => x.ReleaseDate)
            .HasColumnName("release_date");
        
        builder.Property(x => x.CoverImageUrl)
            .HasColumnName("cover_image_url");
        
        builder.Property(x => x.HtmlContentUrl)
            .HasColumnName("html_url");
    }
}