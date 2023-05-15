namespace Library.DataAccess.Entities;

public class User : Entity
{
    public string Username { get; set; }
    
    // one-to-many reviews
    
    public ICollection<Review> Reviews { get; set; }
}