namespace Library.Application.Models.Book.Explore.Category;

public class BooksForCategoryResponseModel
{
    public int BookshelfId { get; set; }
    
    public string BookshelfTitle { get; set; }
    
    public List<ExploreCategoryResponseModel> Categories { get; set; }
}