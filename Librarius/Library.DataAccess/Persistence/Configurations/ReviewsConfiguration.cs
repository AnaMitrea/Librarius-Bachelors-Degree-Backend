﻿using Library.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class ReviewsConfiguration  : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("booksReviews");
        
        builder.Property(x => x.Id)
            .HasColumnName("id");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(x => x.Likes)
            .HasColumnName("likes");
        
        builder.Property(x => x.Timestamp)
            .HasColumnName("timestamp");

        // 1 user has * reviews, userId as Foreign Key
        builder.Property(x => x.UserId).IsRequired();
        builder.HasOne(review => review.User)
            .WithMany(user => user.Reviews)
            .HasForeignKey(review => review.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // 1 book has * reviews, bookId as Foreign Key
        builder.Property(x => x.BookId).IsRequired();
        builder.HasOne(review => review.Book)
            .WithMany(book => book.Reviews)
            .HasForeignKey(review => review.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}