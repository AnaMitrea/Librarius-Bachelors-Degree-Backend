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
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
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
            .HasDefaultValue("User")
            .IsRequired();
        
        builder.Property(x => x.Level)
            .HasColumnName("level")
            .HasDefaultValue("Beginner")
            .IsRequired();

        builder.Property(x => x.Points)
            .HasColumnName("points")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.LastLogin)
            .HasColumnName("last_login");

        builder.Property(x => x.LongestStreak)
            .HasColumnName("longest_streak");
        
        builder.Property(x => x.CurrentStreak)
            .HasColumnName("current_streak");
    }
}