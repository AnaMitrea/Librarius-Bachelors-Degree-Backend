using Library.Application.Models.Book.Author;

namespace Library.Application.Models.Book;

public class BookNoCategoriesResponseModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public AuthorResponseModel Author { get; set; }
    
    public string Link { get; set; }

    public string Language { get; set; }

    public string ReleaseDate { get; set; }

    public string CoverImageUrl { get; set; }
}