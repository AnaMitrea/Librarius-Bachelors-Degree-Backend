using Library.Application.Models.Bookshelf.Response;

namespace Library.Application.Models.Category;

public class CategoryWithBookshelfResponseModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public BookshelfResponseModel Bookshelf { get; set; }
}