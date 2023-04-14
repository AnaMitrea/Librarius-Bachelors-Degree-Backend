using Library.Application.Models.Book;

namespace Library.Application.Services;

public interface IBookService
{
    Task<BookResponseModel> GetBookWithCategoryByIdAsync(int id);

    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingNowBooksAsync();
    
    Task<IEnumerable<BookTrendingResponseModel>> GetTrendingWeekBooksAsync();
}