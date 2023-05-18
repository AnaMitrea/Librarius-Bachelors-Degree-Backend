using Library.Application.Models.BookCategory;
using Library.Application.Models.Bookshelf;

namespace Library.Application.Models.Book.Home;

public class ExploreBookResponseModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public string Author { get; set; }
    
    public string CoverImageUrl { get; set; }
    
    // many-to-many: books - categories
    public IEnumerable<BookCategoryResponseModel> BookCategories { get; set; }
    
    public BookshelfResponseModel Bookshelf { get; set; }
}