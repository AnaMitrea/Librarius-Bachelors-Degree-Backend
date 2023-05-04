using Identity.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Persistence.Configurations;

public class TrophyConfiguration : IEntityTypeConfiguration<Trophy>
{
    public void Configure(EntityTypeBuilder<Trophy> builder)
    {
        builder.ToTable("trophies");

        builder.Property(x => x.Id)
            .HasColumnName("id");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired();

        builder.Property(x => x.Category)
            .HasColumnName("category")
            .IsRequired();
        
        builder.Property(x => x.Instructions)
            .HasColumnName("instructions")
            .IsRequired();
        
        builder.Property(x => x.ImageSrcPath)
            .HasColumnName("image_src_path")
            .IsRequired();
    }
}