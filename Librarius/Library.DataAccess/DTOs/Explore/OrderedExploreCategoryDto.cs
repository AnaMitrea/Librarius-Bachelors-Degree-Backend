using Library.DataAccess.Entities;
using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.DTOs.Explore;

public class OrderedExploreCategoryDto : Entity
{
    public string Title { get; set; }
    
    public int TotalBooks { get; set; }
    
    public Dictionary<string, List<Book>> Books { get; set; }
}