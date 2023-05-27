using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.DTOs;

public class BookshelfWithBooksDto
{
    public int TotalBooks { get; set; }
    
    public List<Book> Books { get; set; }
}
