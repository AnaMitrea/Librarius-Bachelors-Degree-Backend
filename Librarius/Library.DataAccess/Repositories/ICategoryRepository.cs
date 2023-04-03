using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    
    // TODO add CRUD operations
}