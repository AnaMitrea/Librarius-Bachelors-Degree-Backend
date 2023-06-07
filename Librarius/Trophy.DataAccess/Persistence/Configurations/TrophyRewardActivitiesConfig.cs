using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trophy.DataAccess.Entities;

namespace Trophy.DataAccess.Persistence.Configurations;

public class TrophyRewardActivitiesConfig : IEntityTypeConfiguration<TrophyRewardActivities>
{
    public void Configure(EntityTypeBuilder<TrophyRewardActivities> builder)
    {
        builder.ToTable("trophyRewardActivities");

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
        
        builder.Property(x => x.MinimumCriterionText)
            .HasColumnName("minimum_criterion_text")
            .IsRequired();

        builder.Property(x => x.UserProgress)
            .HasColumnName("user_progress")
            .HasDefaultValue(0)
            .IsRequired();

        // many-to-many for trophies and accounts
        
        builder
            .HasOne(trophyAcc => trophyAcc.Trophy)
            .WithMany(trophy => trophy.TrophiesRewardActivities)
            .HasForeignKey(trophyAcc => trophyAcc.TrophyId);
        
        builder
            .HasOne<TrophyRewardActivities>()
            .WithMany()
            .HasForeignKey(trophyAcc => trophyAcc.UserId);
    }
}