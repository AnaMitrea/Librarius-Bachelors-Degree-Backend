using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.Repositories;

public interface IBookshelfRepository
{
    Task<List<Bookshelf>> GetAllAsync();
    
    Task<List<Bookshelf>> Get4CategoriesForHomeExploreAsync();
    
    Task<List<Bookshelf>> GetAllWithCategoryAsync();
}