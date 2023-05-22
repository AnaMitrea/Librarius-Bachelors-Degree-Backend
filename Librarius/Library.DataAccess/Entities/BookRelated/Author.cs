using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.Entities.BookRelated;

public class Author : Entity
{
    public string Name { get; set; }
    
    // 1 author to * books
    public ICollection<Book> Books { get; set; }
    
    // one-to-many user subscribers
    public ICollection<Subscription> Subscribers { get; set; }
}