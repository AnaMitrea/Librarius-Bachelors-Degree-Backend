using Identity.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");
        
        builder.Property(x => x.Id)
            .HasColumnName("id");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Email)
            .HasColumnName("email")
            .IsRequired();

        builder.Property(x => x.Username)
            .HasColumnName("username")
            .IsRequired();
        
        builder.Property(x => x.Password)
            .HasColumnName("password")
            .IsRequired();
        
        builder.Property(x => x.Role)
            .HasColumnName("role")
            .IsRequired();
        
        builder.Property(x => x.Level)
            .HasColumnName("level")
            .IsRequired();

        builder.Property(x => x.Points)
            .HasColumnName("points")
            .IsRequired();

        builder.Property(x => x.LastLogin)
            .HasColumnName("last_login");

        builder.Property(x => x.LongestStreak)
            .HasColumnName("longest_streak");
    }
}