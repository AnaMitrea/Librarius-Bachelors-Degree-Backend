using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trophy.DataAccess.Entities;

namespace Trophy.DataAccess.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<TrophyUser>
{
    public void Configure(EntityTypeBuilder<TrophyUser> builder)
    {
        builder.ToTable("trophyUsers");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id");
        
        builder.Property(x => x.Username)
            .HasColumnName("username")
            .IsRequired();
    }
}