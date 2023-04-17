using Library.DataAccess.Persistence;

namespace Library.DataAccess.Repositories.Implementations;

public class BookCategoryRepository : IBookCategoryRepository
{
    private readonly DatabaseContext _databaseContext;

    public BookCategoryRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    // TODO implement CRUD operations defined in interface
}