namespace Library.DataAccess.Entities.BookRelated;

public class Subscription : Entity
{
    public int UserId { get; set; }
    
    public User.User User { get; set; }
    
    public int AuthorId { get; set; }
    
    public Author Author { get; set; }
}