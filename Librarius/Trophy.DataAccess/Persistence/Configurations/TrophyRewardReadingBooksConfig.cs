using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trophy.DataAccess.Entities;

namespace Trophy.DataAccess.Persistence.Configurations;

public class TrophyRewardReadingBooksConfig : IEntityTypeConfiguration<TrophyRewardReadingBooks>
{
    public void Configure(EntityTypeBuilder<TrophyRewardReadingBooks> builder)
    {
        builder.ToTable("trophyRewardReadingBooks");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(x => x.TrophyId)
            .HasColumnName("trophy_id")
            .IsRequired();
        
        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(x => x.IsWon)
            .HasColumnName("is_won")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.Property(x => x.MinimumCriterionNumber)
            .HasColumnName("minimum_criterion_number")
            .IsRequired();

        builder.Property(x => x.UserProgress)
            .HasColumnName("user_progress")
            .HasDefaultValue(0)
            .IsRequired();

        // many-to-many for trophies and accounts
        
        builder
            .HasOne(trophyAcc => trophyAcc.Trophy)
            .WithMany(trophy => trophy.TrophiesRewardReadingBooks)
            .HasForeignKey(trophyAcc => trophyAcc.TrophyId);
        
        builder
            .HasOne<TrophyRewardReadingBooks>()
            .WithMany()
            .HasForeignKey(trophyAcc => trophyAcc.UserId);
    }
}