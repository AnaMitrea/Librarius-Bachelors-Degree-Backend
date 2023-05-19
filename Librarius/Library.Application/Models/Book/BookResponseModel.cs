using Library.Application.Models.Book.Author;
using Library.Application.Models.BookCategory;

namespace Library.Application.Models.Book;

public class BookResponseModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public AuthorResponseModel Author { get; set; }
    
    public string Link { get; set; }

    public string Language { get; set; }

    public string ReleaseDate { get; set; }

    public string CoverImageUrl { get; set; }

    public string HtmlContentUrl { get; set; }

    // many-to-many: books - categories
    public IEnumerable<BookCategoryResponseModel> BookCategories { get; set; }
}