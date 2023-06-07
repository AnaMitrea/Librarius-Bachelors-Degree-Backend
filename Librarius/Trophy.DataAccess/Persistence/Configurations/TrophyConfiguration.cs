using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trophy.DataAccess.Persistence.Configurations;

public class TrophyConfiguration : IEntityTypeConfiguration<Entities.Trophy>
{
    public void Configure(EntityTypeBuilder<Entities.Trophy> builder)
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
        
        builder.Property(x => x.MinimumCriterionNumber)
            .HasColumnName("minimum_criterion_number")
            .IsRequired(false);
        
        builder.Property(x => x.MinimumCriterionText)
            .HasColumnName("minimum_criterion_text")
            .IsRequired(false);
    }
}