using Library.Application.Models.Book;

namespace Library.Application.Services;

public interface IBookService
{
    Task<BookResponseModel> GetBookByIdAsync(int id);

    Task<BookResponseModel> GetBookWithCategoryByIdAsync(int id);
}