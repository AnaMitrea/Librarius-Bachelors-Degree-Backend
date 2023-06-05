namespace Library.Application.Models.Book.Explore.Category;

public class OrderedExploreCategoryResponseModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public int TotalBooks { get; set; }
    
    public Dictionary<string, List<BookNoCategoriesResponseModel>> Books { get; set; }
}