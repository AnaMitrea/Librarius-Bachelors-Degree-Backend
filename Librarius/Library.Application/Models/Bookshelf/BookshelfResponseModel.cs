using Library.Application.Models.Category;

namespace Library.Application.Models.Bookshelf;

public class BookshelfResponseModel 
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public IEnumerable<CategoryResponseModel> Categories { get; set; }
}