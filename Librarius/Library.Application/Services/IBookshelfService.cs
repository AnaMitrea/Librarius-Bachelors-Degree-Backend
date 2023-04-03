using Library.Application.Models.Bookshelf;

namespace Library.Application.Services;

public interface IBookshelfService
{
    Task<List<BookshelfResponseModel>> GetAllAsync();
    
}