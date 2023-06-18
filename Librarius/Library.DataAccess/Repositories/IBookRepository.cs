using Library.DataAccess.DTOs;
using Library.DataAccess.DTOs.Explore;
using Library.DataAccess.Entities.Library;
using Library.DataAccess.Entities.User;

namespace Library.DataAccess.Repositories;

public interface IBookRepository
{
    Task<Book?> GetBookByIdAsync(int id);

    Task<Book?> GetBookWithCategoryByIdAsync(int id);
    
    Task<int> GetCategoryIdOfBookByIdAsync(int id);

    Task<IEnumerable<Book>> GetBooksForAllBookshelves();

    Task<Dictionary<string, BookshelfWithBooksDto>> GetBooksGroupedByBookshelf(int? maxResults, string? title);
    
    Task<List<BookshelfCategoryWithBooksDto>> GetBooksGroupedByCategoryAndBookshelf(int? maxResults, string? title);

    Task<List<BookshelfCategoryWithBooksDto>> GetGroupedCategoryAndBookshelf(string? title);

    Task<Dictionary<string, BookshelfWithBooksDto>> GetGroupedBookshelves(string? title);

    Task<Dictionary<string, OrderedBookshelfWithBooksDto>> GetOrderedBooksGroupedByBookshelf(int? maxResults,
        string? title);

    Task<List<OrderedBookshelfCategoryWithBooksDto>> GetOrderedBooksGroupedByCategories(string startFrom, string bookshelfTitle, string categoryTitle, int? maxResults);
    
    Task<BookWithContentDto> GetReadingBookByIdAsync(int id);

    Task<int> CountWordsInResponseAsync(int bookId);

    Task<ReadingTimeResponseDto> GetReadingTimeOfBookContent(int bookId);

    Task<bool> CheckIsBookFinishedReading(int bookId, string username);

    Task<UserBookReadingTracker> GetUserReadingTimeSpentAsync(int bookId, string username);

    Task<int> GetUserTotalReadingTimeSpentAsync(int userId);
    
    Task<int> GetUserTotalReadingTimeSpentByUsernameAsync(string username);
    
    Task<int> UpdateUserReadingTimeSpentAsync(int bookId, string username, int timeSpent);
    
    Task<bool> SetFinishedReadingBookByIdAsync(int bookId, string username, int timeSpent);

    Task<IEnumerable<Book>> GetTrendingNowBooksAsync();
    
    Task<IEnumerable<Book>> GetTrendingWeekBooksAsync();
    Task<IEnumerable<Book>> SearchBooksByFilterAsync(string searchByKey, int maxResults);
    Task<bool> SetOrRemoveFavoriteBookAsync(string username, int bookId);
}