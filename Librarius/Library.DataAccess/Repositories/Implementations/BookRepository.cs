using Library.DataAccess.Entities;
using Library.DataAccess.Persistance;

namespace Library.DataAccess.Repositories.Implementations;

public class BookRepository : IBookRepository
{
    private readonly DatabaseContext _databaseContext;
    
    public BookRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task<Book> GetBookByIdAsync(int id)
    {
        var book = await _databaseContext.Books.FindAsync(id);
        if (book == null)
        {
            throw new Exception("The book id is invalid");
        }
        
        return book;
    }
    
    // TODO implement CRUD operations defined in interface
}