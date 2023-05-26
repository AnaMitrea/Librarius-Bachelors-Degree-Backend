using Library.DataAccess.Entities.BookRelated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class UserCompletedBooksConfiguration : IEntityTypeConfiguration<UserCompletedBooks>
{
    public void Configure(EntityTypeBuilder<UserCompletedBooks> builder)
    {
        builder.ToTable("userCompletedBooks");
        
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
            .HasDefaultValue(0)
            .IsRequired();
        
        // many-to-many for books - users
        
        builder
            .HasOne(compl => compl.Book)
            .WithMany(book => book.CompletedBooks)
            .HasForeignKey(compl => compl.BookId);
        
        builder
            .HasOne(compl => compl.User)
            .WithMany(user => user.CompletedBooks)
            .HasForeignKey(compl => compl.UserId);
    }
}