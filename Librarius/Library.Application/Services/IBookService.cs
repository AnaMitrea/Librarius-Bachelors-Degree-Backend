using Library.Application.Models.Book;
using Library.Application.Models.Book.Explore.Bookshelf;
using Library.Application.Models.Book.Explore.Category;
using Library.Application.Models.Book.Favorite;
using Library.Application.Models.Book.Home;
using Library.Application.Models.Book.Reading;
using Library.Application.Models.Book.Reading.Response;
using Library.Application.Models.Book.Trending;
using Library.Application.Models.SearchBar;
using Library.DataAccess.DTOs;

namespace Library.Application.Services;

public interface IBookService
{
    Task<BookResponseModel> GetBookWithCategoryByIdAsync(int id);

    Task<IEnumerable<ExploreBookResponseModel>> GetBooksForAllBookshelves();

    Task<Dictionary<string, BooksForBookshelfResponseModel>> GetBooksGroupedByBookshelf(int? maxResults, string? title);

    Task<Dictionary<string, BooksForBookshelfResponseModel>> GetGroupedBookshelves(string? title);
    
    Task<Dictionary<string, OrderedBooksForBookshelfResponseModel>> GetOrderedBooksGroupedByBookshelf(int? maxResults,
        string? title);

    Task<List<BooksForCategoryResponseModel>> GetBooksGroupedByCategoryAndBookshelf(int? maxResults, string? title);
    
    
    Task<List<OrderedBookshelfCategoryBooksResponseModel>> GetOrderedBooksGroupedByCategories(string startFrom, int? maxResults, string? title);

    Task<List<BooksForCategoryResponseModel>> GetGroupedCategoryAndBookshelf(string? title);

    Task<BookReadingResponseModel> GetReadingBookByIdAsync(int id);

    Task<int> GetBookContentWordCount(int id);

    Task<ReadingTimeResponseDto> GetReadingTimeOfBookContent(int id);

    Task<bool> CheckIsBookFinishedReading(int bookId, string username);

    Task<ReadingTimeSpentResponseModel> GetUserReadingTimeSpentAsync(ReadingRequestModel requestModel, string username);
    
    Task<bool> UpdateUserReadingTimeSpentAsync(UserReadingBookRequestModel requestModel, string username);

    Task<bool> SetFinishedReadingBookByIdAsync(UserReadingBookRequestModel requestModel, string username);

    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingNowBooksAsync();
    
    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingWeekBooksAsync();
    
    Task<IEnumerable<BookMinimalResponseModel>> SearchBooksByFilterAsync(SearchBarRequestModel requestModelSearchBy);

    Task<bool> SetOrRemoveFavoriteBookAsync(BookFavoriteRequestModel requestModel, string username);
}