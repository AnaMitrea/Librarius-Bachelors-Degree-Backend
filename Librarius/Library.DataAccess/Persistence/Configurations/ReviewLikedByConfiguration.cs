using Library.DataAccess.Entities;
using Library.DataAccess.Entities.BookRelated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class ReviewLikedByConfiguration : IEntityTypeConfiguration<ReviewLikedBy>
{
    public void Configure(EntityTypeBuilder<ReviewLikedBy> builder)
    {
        builder.ToTable("bookReviewsLikedBy");
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        builder.HasKey(x => x.Id);

        // 1 user has * likes, userId as Foreign Key
        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        builder.HasOne(like => like.User)
            .WithMany(user => user.Likes)
            .HasForeignKey(like => like.UserId);
        
        // 1 review has * likes, reviewId as Foreign Key
        builder.Property(x => x.ReviewId)
            .HasColumnName("review_id")
            .IsRequired();
        builder.HasOne(like => like.Review)
            .WithMany(review => review.Likes)
            .HasForeignKey(like => like.ReviewId);
    }
}