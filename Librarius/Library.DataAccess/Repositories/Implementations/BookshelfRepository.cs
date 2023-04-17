using Library.DataAccess.Entities;
using Library.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public class BookshelfRepository : IBookshelfRepository
{
    private readonly DatabaseContext _databaseContext;

    public BookshelfRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<List<Bookshelf>> GetAllAsync()
    {
        return await _databaseContext.Bookshelves.ToListAsync();
    }
}