using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.Entities.User;

public class UserFavoriteBook : Entity
{
    public int UserId { get; set; }
    
    public User User { get; set; }
    
    public int BookId { get; set; }
    
    public Book Book { get; set; }
}