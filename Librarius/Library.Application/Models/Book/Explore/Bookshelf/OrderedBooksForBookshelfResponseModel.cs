using Library.Application.Models.Book.Home;

namespace Library.Application.Models.Book.Explore.Bookshelf;

public class OrderedBooksForBookshelfResponseModel
{
    public int Id { get; set; }
    
    public int TotalBooks { get; set; }
    
    public Dictionary<string, List<BookshelfBookResponseModel>> Books { get; set; }
}
