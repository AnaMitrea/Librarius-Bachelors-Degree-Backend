using Library.DataAccess.DTOs;
using Library.DataAccess.Entities.Library;
using Library.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public class BookshelfRepository : IBookshelfRepository
{
    private readonly DatabaseContext _dbContext;

    public BookshelfRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    public async Task<List<Bookshelf>> GetAllAsync()
    {
        return await _dbContext.Bookshelves.ToListAsync();
    }

    public async Task<List<Bookshelf>> GetAllWithCategoryAsync()
    {
        return await _dbContext.Bookshelves.Include(bookshelf => bookshelf.Categories).ToListAsync();
    }
}