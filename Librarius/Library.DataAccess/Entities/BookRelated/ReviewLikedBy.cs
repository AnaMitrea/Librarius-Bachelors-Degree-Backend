namespace Library.DataAccess.Entities.BookRelated;

public class ReviewLikedBy : Entity
{
    // many-to-one Review
    public int ReviewId { get; set; }
    
    public Review Review { get; set; }
    
    // many-to-one User
    public int UserId { get; set; }
    
    public User.User User { get; set; }
}