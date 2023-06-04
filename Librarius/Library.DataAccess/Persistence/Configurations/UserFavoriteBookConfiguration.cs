using Library.DataAccess.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataAccess.Persistence.Configurations;

public class UserFavoriteBookConfiguration : IEntityTypeConfiguration<UserFavoriteBook>
{
    public void Configure(EntityTypeBuilder<UserFavoriteBook> builder)
    {
        builder.ToTable("userWishlistBooks");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        
        // many-to-many for books - users
        
        builder.Property(x => x.BookId)
            .HasColumnName("book_id")
            .IsRequired();
        
        builder
            .HasOne(compl => compl.Book)
            .WithMany(book => book.UserFavoriteBooks)
            .HasForeignKey(compl => compl.BookId);
        
        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder
            .HasOne(compl => compl.User)
            .WithMany(user => user.FavoriteBooks)
            .HasForeignKey(compl => compl.UserId);
    }
}