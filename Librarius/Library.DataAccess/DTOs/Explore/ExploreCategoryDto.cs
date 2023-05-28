using Library.DataAccess.Entities;
using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.DTOs.Explore;

public class ExploreCategoryDto : Entity
{
    public string Title { get; set; }
    
    public int TotalBooks { get; set; }
    
    public List<Book> Books { get; set; }
}