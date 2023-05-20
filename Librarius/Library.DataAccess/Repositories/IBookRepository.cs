using Library.DataAccess.DTOs;
using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.Repositories;

public interface IBookRepository
{
    Task<Book?> GetBookByIdAsync(int id);

    Task<Book?> GetBookWithCategoryByIdAsync(int id);

    Task<IEnumerable<Book>> GetBooksForAllBookshelves();

    Task<Dictionary<string, List<Book>>> GetBooksGroupedByBookshelf();

    Task<BookWithContentDto> GetReadingBookByIdAsync(int id);

    Task<int> CountWordsInResponseAsync(int bookId);

    Task<ReadingTimeResponseDto> GetReadingTimeOfBookContent(int bookId);

    Task<bool> SetFinishedReadingBookByIdAsync(int bookId, string username, int timeSpent);

    Task<IEnumerable<Book?>> GetTrendingNowBooksAsync();
    
    Task<IEnumerable<Book?>> GetTrendingWeekBooksAsync();
    
}