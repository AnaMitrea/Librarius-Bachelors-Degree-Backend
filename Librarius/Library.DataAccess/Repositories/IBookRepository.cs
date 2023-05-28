using Library.DataAccess.DTOs;
using Library.DataAccess.DTOs.Explore;
using Library.DataAccess.Entities.Library;

namespace Library.DataAccess.Repositories;

public interface IBookRepository
{
    Task<Book?> GetBookByIdAsync(int id);

    Task<Book?> GetBookWithCategoryByIdAsync(int id);

    Task<IEnumerable<Book>> GetBooksForAllBookshelves();

    Task<Dictionary<string, BookshelfWithBooksDto>> GetBooksGroupedByBookshelf(int maxResults);
    
    Task<List<BookshelfCategoryWithBooksDto>> GetBooksGroupedByCategoryAndBookshelf(int maxResults);

    Task<BookWithContentDto> GetReadingBookByIdAsync(int id);

    Task<int> CountWordsInResponseAsync(int bookId);

    Task<ReadingTimeResponseDto> GetReadingTimeOfBookContent(int bookId);

    Task<bool> CheckIsBookFinishedReading(int bookId, string username);
    
    Task<bool> SetFinishedReadingBookByIdAsync(int bookId, string username, int timeSpent);

    Task<IEnumerable<Book?>> GetTrendingNowBooksAsync();
    
    Task<IEnumerable<Book?>> GetTrendingWeekBooksAsync();
}