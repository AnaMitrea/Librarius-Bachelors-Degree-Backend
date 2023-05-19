using Library.DataAccess.Entities.BookRelated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("authors");
        
        builder.Property(x => x.Id)
            .HasColumnName("id");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();
    }
}