using Library.DataAccess.Entities.BookRelated;
using Library.DataAccess.Entities.User;

namespace Library.DataAccess.Entities.Library;

public class Book : Entity
{
    public string Title { get; set; }
    
    public string Link { get; set; }

    public string Language { get; set; }

    public string ReleaseDate { get; set; }

    public string CoverImageUrl { get; set; }

    public string? HtmlContentUrl { get; set; }
    
    public string? HtmlAsSubmittedUrl { get; set; }
    
    public string? HtmlNoImagesUrl { get; set; }
    
    public string? PlainTextUrl { get; set; }
    
    // many-to-one author
    public int AuthorId { get; set; }
    public Author Author { get; set; }

    // many-to-many: books - categories
    public IEnumerable<BookCategory> BookCategories { get; set; }
    
    // one-to-many reviews
    public ICollection<Review> Reviews { get; set; }
    
    // many-to-many user readers
    public ICollection<UserBookReadingTracker> ReadingBooksTracker { get; set; }
}


