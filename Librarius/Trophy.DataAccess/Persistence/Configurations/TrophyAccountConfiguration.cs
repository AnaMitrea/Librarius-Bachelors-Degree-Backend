using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trophy.DataAccess.Entities;

namespace Trophy.DataAccess.Persistence.Configurations;

public class TrophyAccountConfiguration : IEntityTypeConfiguration<TrophyUserReward>
{
    public void Configure(EntityTypeBuilder<TrophyUserReward> builder)
    {
        builder.ToTable("trophyUserReward");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(x => x.TrophyId)
            .HasColumnName("trophy_id")
            .IsRequired();
        
        builder.Property(x => x.IsWon)
            .HasColumnName("isWon")
            .IsRequired();
        
        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        // many-to-many for trophies and accounts
        
        builder
            .HasOne(trophyAcc => trophyAcc.Trophy)
            .WithMany(trophy => trophy.TrophyAccounts)
            .HasForeignKey(trophyAcc => trophyAcc.TrophyId);
        
        builder
            .HasOne(trophyAcc => trophyAcc.User)
            .WithMany(account => account.Trophies)
            .HasForeignKey(trophyAcc => trophyAcc.UserId);
    }
}