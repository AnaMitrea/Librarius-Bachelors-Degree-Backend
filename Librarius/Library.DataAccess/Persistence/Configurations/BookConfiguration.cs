using Library.DataAccess.Entities.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        // builder.ToTable("books");
        builder.ToTable("books_modified");
        
        builder.Property(x => x.Id)
            .HasColumnName("id");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired();
        
        builder.Property(x => x.AuthorId)
            .HasColumnName("author_id")
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
        
        builder.Property(x => x.HtmlAsSubmittedUrl)
            .HasColumnName("html_as_submitted_url")
            .IsRequired(false);
        
        builder.Property(x => x.HtmlNoImagesUrl)
            .HasColumnName("html_no_images_url")
            .IsRequired(false) ;
        
        builder.Property(x => x.PlainTextUrl)
            .HasColumnName("plain_text_url")
            .IsRequired(false) ;
        
        // 1 author has * books, author_id as foreign key
        builder.Property(x => x.AuthorId).IsRequired();
        builder.HasOne(book => book.Author)
            .WithMany(author => author.Books)
            .HasForeignKey(book => book.AuthorId);
    }
}