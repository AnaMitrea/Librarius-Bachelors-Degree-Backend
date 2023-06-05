namespace Library.Application.Models.Book.Explore.Category;

public class OrderedBookshelfCategoryBooksResponseModel
{
    public int BookshelfId { get; set; }
    
    public string BookshelfTitle { get; set; }
    
    public List<OrderedExploreCategoryResponseModel> Categories { get; set; }
}