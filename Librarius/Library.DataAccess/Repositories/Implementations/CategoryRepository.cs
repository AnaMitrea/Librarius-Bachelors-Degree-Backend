using Library.DataAccess.Entities;
using Library.DataAccess.Entities.Library;
using Library.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly DatabaseContext _dbContext;

    public CategoryRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }
    
    public async Task<List<Category>>  GetAllAsync()
    {
        return await _dbContext.Categories.Include(category => category.Bookshelf).ToListAsync();
    }
}