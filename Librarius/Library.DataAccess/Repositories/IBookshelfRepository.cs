using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IBookshelfRepository
{
    Task<List<Bookshelf>> GetAllAsync();

    // TODO add CRUD operations
}