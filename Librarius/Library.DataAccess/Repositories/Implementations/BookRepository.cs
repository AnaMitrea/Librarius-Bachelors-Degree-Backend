using Library.DataAccess.Entities;
using Library.DataAccess.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repositories.Implementations;

public class BookRepository : IBookRepository
{
    private readonly DatabaseContext _databaseContext;
    
    public BookRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task<Book?> GetBookByIdAsync(int bookId)
    {
        var book = (await _databaseContext.Books.ToListAsync()).SingleOrDefault(book => book.Id == bookId);
        
        if (book == default)
        {
            throw new Exception("The book id is invalid");
        }
        
        return book;
    }
    
    public async Task<Book?> GetBookWithCategoryByIdAsync(int bookId)
    {
        var book = (await _databaseContext.Books
            .Include(x => x.BookCategories)
            .ThenInclude(x => x.Category)
            .ToListAsync()).SingleOrDefault(book => book.Id == bookId);
        
        if (book == default)
        {
            throw new Exception("The book id is invalid");
        }
        
        return book;
    }
    
    // TODO implement CRUD operations defined in interface
}