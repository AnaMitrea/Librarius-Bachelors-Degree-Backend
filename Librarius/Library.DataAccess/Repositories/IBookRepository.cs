using Library.DataAccess.DTOs;
using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IBookRepository
{
    Task<Book?> GetBookByIdAsync(int id);

    Task<Book?> GetBookWithCategoryByIdAsync(int id);

    Task<BookWithContent?> GetReadingBookByIdAsync(int id);

    Task<IEnumerable<Book?>> GetTrendingNowBooksAsync();
    
    Task<IEnumerable<Book?>> GetTrendingWeekBooksAsync();

    // TODO add CRUD operations
}