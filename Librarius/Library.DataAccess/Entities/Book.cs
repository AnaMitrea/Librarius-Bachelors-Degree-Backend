namespace Library.DataAccess.Entities;

public class Book : Entity
{
    public string Title { get; set; }

    public string Author { get; set; }
    
    public string Link { get; set; }

    public string Language { get; set; }

    public string ReleaseDate { get; set; }

    public string CoverImageUrl { get; set; }

    public string HtmlContentUrl { get; set; }

    // many-to-many: books - categories
    public IEnumerable<BookCategory> BookCategories { get; set; }
    
    // one-to-many reviews
    public ICollection<Review> Reviews { get; set; }
}


