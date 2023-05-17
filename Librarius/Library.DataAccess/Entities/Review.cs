namespace Library.DataAccess.Entities;

public class Review : Entity
{
    public string Content { get; set; }

    public string Timestamp { get; set; }
    
    public int Rating { get; set; }
    
    // !!! Denormalized the data in order to remove the need to calculate the likes count dynamically each time
    public int LikesCount { get; set; }
    
    // many-to-one Book
    public int BookId { get; set; }
    
    public Book Book { get; set; }
    
    // many-to-one User
    public int UserId { get; set; }
    
    public User User { get; set; }
    
    // one-to-many Likes
    public ICollection<ReviewLikedBy> Likes { get; set; }
}