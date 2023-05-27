using Library.DataAccess.Entities;
using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.DTOs;

public class BookshelfWithBooksDto : Entity
{
    public int TotalBooks { get; set; }
    
    public List<Book> Books { get; set; }
}
