using Library.Application.Models.Book.Home;

namespace Library.Application.Models.Book.Explore.Bookshelf;

public class BooksForBookshelfResponseModel
{
    public int Id { get; set; }
    
    public int TotalBooks { get; set; }
    
    public List<BookshelfBookResponseModel> Books { get; set; }
}
