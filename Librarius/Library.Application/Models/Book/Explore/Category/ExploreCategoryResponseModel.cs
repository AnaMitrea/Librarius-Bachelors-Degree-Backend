namespace Library.Application.Models.Book.Explore.Category;

public class ExploreCategoryResponseModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public int TotalBooks { get; set; }
    
    public List<BookNoCategoriesResponseModel> Books { get; set; }
}