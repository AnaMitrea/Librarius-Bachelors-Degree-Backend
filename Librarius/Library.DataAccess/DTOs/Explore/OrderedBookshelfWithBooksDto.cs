using Library.DataAccess.Entities;
using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.DTOs.Explore;

public class OrderedBookshelfWithBooksDto : Entity
{
    public int TotalBooks { get; set; }
    
    public Dictionary<string, List<Book>> Books { get; set; }
}
