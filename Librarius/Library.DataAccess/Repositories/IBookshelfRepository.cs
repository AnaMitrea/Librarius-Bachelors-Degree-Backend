using Library.DataAccess.Entities;
using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.Repositories;

public interface IBookshelfRepository
{
    Task<List<Bookshelf>> GetAllAsync();
    
    Task<List<Bookshelf>> GetAllWithCategoryAsync();
}