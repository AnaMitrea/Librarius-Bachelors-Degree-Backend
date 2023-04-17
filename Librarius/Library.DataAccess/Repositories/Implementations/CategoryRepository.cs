using Library.DataAccess.Entities;
using Library.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly DatabaseContext _databaseContext;

    public CategoryRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    // TODO implement CRUD operations defined in interface
    public async Task<List<Category>>  GetAllAsync()
    {
        return await _databaseContext.Categories.ToListAsync();
    }
}