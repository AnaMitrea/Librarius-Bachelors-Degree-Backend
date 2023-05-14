using Library.Application.Models.Book;

namespace Library.Application.Services;

public interface IBookService
{
    Task<BookResponseModel> GetBookWithCategoryByIdAsync(int id);

    Task<IEnumerable<ExploreBookResponseModel>> GetBooksForAllBookshelves();

    Task<Dictionary<string, List<BookResponseModel>>> GetBooksGroupedByBookshelf();

    Task<BookReadingResponseModel> GetReadingBookByIdAsync(int id);

    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingNowBooksAsync();
    
    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingWeekBooksAsync();
}