using Library.DataAccess.Entities;

namespace Library.DataAccess.DTOs.Explore;

public class BookshelfCategoryWithBooksDto : Entity
{
    public string Title { get; set; }
    
    public List<ExploreCategoryDto> Categories { get; set; }
}