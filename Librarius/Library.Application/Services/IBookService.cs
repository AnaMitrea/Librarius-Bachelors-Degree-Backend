using Library.Application.Models.Book;

namespace Library.Application.Services;

public interface IBookService
{
    Task<BookResponseModel> GetByIdAsync(int id);
}