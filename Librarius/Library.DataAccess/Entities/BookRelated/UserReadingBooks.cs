using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.Entities.BookRelated;

public class UserReadingBooks : Entity
{
    public bool IsBookFinished { get; set; }
    
    public int MinutesSpent { get; set; }
    
    public string? Timestamp { get; set; }
    
    // many-to-one User
    public int UserId { get; set; }
    
    public User User { get; set; }
    
    // many-to-one Book
    public int BookId { get; set; }
    
    public Book Book { get; set; }
}