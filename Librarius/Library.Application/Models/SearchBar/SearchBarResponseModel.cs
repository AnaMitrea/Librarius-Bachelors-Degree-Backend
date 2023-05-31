using Library.Application.Models.Book;
using Library.Application.Models.Book.Author;

namespace Library.Application.Models.SearchBar;

public class SearchBarResponseModel
{
    public IEnumerable<AuthorResponseModel> Authors { get; set; }
    
    public IEnumerable<BookMinimalResponseModel> Books { get; set; }
}