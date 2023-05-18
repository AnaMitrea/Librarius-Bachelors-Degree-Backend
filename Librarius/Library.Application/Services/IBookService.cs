using Library.Application.Models.Book;
using Library.Application.Models.Book.Home;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Trending;

namespace Library.Application.Services;

public interface IBookService
{
    Task<BookResponseModel> GetBookWithCategoryByIdAsync(int id);

    Task<IEnumerable<ExploreBookResponseModel>> GetBooksForAllBookshelves();

    Task<Dictionary<string, List<BookResponseModel>>> GetBooksGroupedByBookshelf();

    Task<BookReadingResponseModel> GetReadingBookByIdAsync(int id);

    Task<int> GetBookContentWordCount(int id);

    Task<bool> SetFinishedReadingBookByIdAsync(CompletedBookRequestModel requestModel, string username);

    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingNowBooksAsync();
    
    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingWeekBooksAsync();
}