using Library.Application.Models.BookCategory;

namespace Library.Application.Models.Book;

public class BookResponseModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public string Author { get; set; }
    
    public string Link { get; set; }

    public string Language { get; set; }

    public string ReleaseDate { get; set; }

    public string CoverImageUrl { get; set; }

    public string HtmlContentUrl { get; set; }

    public string HtmlAsSubmittedContentUrl { get; set; }

    public string HtmlNoImagesContentUrl { get; set; }

    public string PlainTextContentUrl { get; set; }
    
    // many-to-many: books - categories
    public IEnumerable<BookCategoryResponseModel> BookCategories { get; set; }
}