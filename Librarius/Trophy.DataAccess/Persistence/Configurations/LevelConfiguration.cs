using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trophy.DataAccess.Entities;

namespace Trophy.DataAccess.Persistence.Configurations;

public class LevelConfiguration : IEntityTypeConfiguration<Level>
{
    public void Configure(EntityTypeBuilder<Level> builder)
    {
        builder.ToTable("levels");
        
        builder.Property(x => x.Id)
            .HasColumnName("id");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasColumnName("name");
        
        builder.Property(x => x.MinPoints)
            .HasColumnName("min_points");
        
        builder.Property(x => x.MaxPoints)
            .HasColumnName("max_points");
    }
}