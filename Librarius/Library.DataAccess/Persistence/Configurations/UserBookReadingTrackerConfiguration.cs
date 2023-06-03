using Library.DataAccess.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class UserBookReadingTrackerConfiguration : IEntityTypeConfiguration<UserBookReadingTracker>
{
    public void Configure(EntityTypeBuilder<UserBookReadingTracker> builder)
    {
        builder.ToTable("userReadingBooks");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.BookId)
            .HasColumnName("book_id")
            .IsRequired();
        
        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(x => x.MinutesSpent)
            .HasColumnName("minutes_spent")
            .IsRequired();
        
        builder.Property(x => x.IsBookFinished)
            .HasColumnName("is_book_finished")
            .IsRequired();
        
        builder.Property(x => x.Timestamp)
            .HasColumnName("timestamp");
        
        // many-to-many for books - users
        
        builder
            .HasOne(compl => compl.Book)
            .WithMany(book => book.ReadingBooksTracker)
            .HasForeignKey(compl => compl.BookId);
        
        builder
            .HasOne(compl => compl.User)
            .WithMany(user => user.ReadingBooks)
            .HasForeignKey(compl => compl.UserId);
    }
}