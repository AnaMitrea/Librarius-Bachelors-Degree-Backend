using Identity.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Persistence.Configurations;

public class TrophyAccountConfiguration : IEntityTypeConfiguration<TrophyAccount>
{
    public void Configure(EntityTypeBuilder<TrophyAccount> builder)
    {
        builder.ToTable("trophyAccounts");

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
        
        builder.Property(x => x.AccountId)
            .HasColumnName("account_id")
            .IsRequired();

        // many-to-many for trophies and accounts
        
        builder
            .HasOne(trophyAcc => trophyAcc.Trophy)
            .WithMany(trophy => trophy.TrophyAccounts)
            .HasForeignKey(trophyAcc => trophyAcc.TrophyId);
        
        builder
            .HasOne(trophyAcc => trophyAcc.Account)
            .WithMany(account => account.TrophyAccounts)
            .HasForeignKey(trophyAcc => trophyAcc.AccountId);
    }
}