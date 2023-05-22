using Library.DataAccess.Entities.BookRelated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("userAuthorSubscriptions");
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        builder.HasKey(x => x.Id);

        // 1 user has * subscriptions, userId as Foreign Key
        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        builder.HasOne(s => s.User)
            .WithMany(user => user.Subscriptions)
            .HasForeignKey(s => s.UserId);
        
        // 1 Author has * subscribers, authorId as Foreign Key
        builder.Property(x => x.AuthorId)
            .HasColumnName("author_id")
            .IsRequired();
        builder.HasOne(s => s.Author)
            .WithMany(author => author.Subscribers)
            .HasForeignKey(s => s.AuthorId);
    }
}