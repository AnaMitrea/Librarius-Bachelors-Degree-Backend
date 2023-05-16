using Library.DataAccess.Persistence;

namespace Library.DataAccess.Repositories.Implementations;

public class BookCategoryRepository : IBookCategoryRepository
{
    private readonly DatabaseContext _dbContext;

    public BookCategoryRepository(DatabaseContext databaseContext)
    {
        _dbContext = databaseContext;
    }
    
    // TODO implement CRUD operations defined in interface
}