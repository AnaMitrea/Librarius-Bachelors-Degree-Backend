using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.Entities.BookRelated;

public class UserCompletedBooks : Entity
{
    public int TimeSpent { get; set; }
    
    // many-to-one User
    public int UserId { get; set; }
    
    public User User { get; set; }
    
    // many-to-one Book
    public int BookId { get; set; }
    
    public Book Book { get; set; }
}