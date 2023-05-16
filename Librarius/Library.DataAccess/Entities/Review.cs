namespace Library.DataAccess.Entities;

public class Review : Entity
{
    public string Content { get; set; }
    
    public int Likes { get; set; }
    
    public string Timestamp { get; set; }
    
    public int Rating { get; set; }
    
    // many-to-one Book
    public int BookId { get; set; }
    
    public Book Book { get; set; }
    
    // many-to-one User
    public int UserId { get; set; }
    
    public User User { get; set; }
}