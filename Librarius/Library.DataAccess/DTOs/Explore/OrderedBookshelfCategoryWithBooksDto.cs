using Library.DataAccess.Entities;

namespace Library.DataAccess.DTOs.Explore;

public class OrderedBookshelfCategoryWithBooksDto : Entity
{
    public string Title { get; set; }
    
    public List<OrderedExploreCategoryDto> Categories { get; set; }
}