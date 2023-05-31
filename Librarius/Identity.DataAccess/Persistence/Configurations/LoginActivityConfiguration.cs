using Identity.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.Persistence.Configurations;

public class LoginActivityConfiguration : IEntityTypeConfiguration<LoginActivity>
{
    public void Configure(EntityTypeBuilder<LoginActivity> builder)
    {
        builder.ToTable("loginActivity");
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DateTimestamp)
            .HasColumnName("date_timestamp")
            .IsRequired();
        
        // 1 user has * activity login dates, accountId as Foreign Key
        builder.Property(x => x.AccountId)
            .HasColumnName("accountId")
            .IsRequired();
        builder.HasOne(activity => activity.Account)
            .WithMany(account => account.LoginActivities)
            .HasForeignKey(activity => activity.AccountId);
    }
}