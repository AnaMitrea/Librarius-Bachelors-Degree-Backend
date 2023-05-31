using Library.Application.Models.Book.Author;

namespace Library.Application.Models.Book;

public class BookMinimalResponseModel
{
    public string Type => "Book";
        
    public int Id { get; set; }
    
    public string Title { get; set; }

    public AuthorResponseModel Author { get; set; }
    
    public string Link { get; set; }

    public string CoverImageUrl { get; set; }
}