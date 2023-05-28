using Library.Application.Models.Book;
using Library.Application.Models.Book.Explore.Bookshelf;
using Library.Application.Models.Book.Explore.Category;
using Library.Application.Models.Book.Home;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Trending;
using Library.DataAccess.DTOs;

namespace Library.Application.Services;

public interface IBookService
{
    Task<BookResponseModel> GetBookWithCategoryByIdAsync(int id);

    Task<IEnumerable<ExploreBookResponseModel>> GetBooksForAllBookshelves();

    Task<Dictionary<string, BooksForBookshelfResponseModel>> GetBooksGroupedByBookshelf(int maxResults);

    Task<List<BooksForCategoryResponseModel>> GetBooksGroupedByCategoryAndBookshelf(int maxResults);

    Task<BookReadingResponseModel> GetReadingBookByIdAsync(int id);

    Task<int> GetBookContentWordCount(int id);

    Task<ReadingTimeResponseDto> GetReadingTimeOfBookContent(int id);

    Task<bool> CheckIsBookFinishedReading(int bookId, string username);

    Task<bool> SetFinishedReadingBookByIdAsync(CompletedBookRequestModel requestModel, string username);

    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingNowBooksAsync();
    
    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingWeekBooksAsync();
}