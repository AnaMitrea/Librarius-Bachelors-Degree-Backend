using Library.DataAccess.Entities.BookRelated;

namespace Library.DataAccess.Entities;

public class User : Entity
{
    public string Username { get; set; }
    
    // one-to-many reviews
    public ICollection<Review> Reviews { get; set; }
    
    // one-to-many likes
    public ICollection<ReviewLikedBy> Likes { get; set; }
    
    // one-to-many author subscriptions
    public ICollection<Subscription> Subscriptions { get; set; }

    // many-to-many completed books
    public ICollection<UserCompletedBooks> CompletedBooks { get; set; }
}