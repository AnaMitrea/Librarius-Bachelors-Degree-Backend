using Library.DataAccess.Entities;
using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
}