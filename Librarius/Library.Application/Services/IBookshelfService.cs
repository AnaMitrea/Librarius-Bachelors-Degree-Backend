using Library.Application.Models.Bookshelf.Response;

namespace Library.Application.Services;

public interface IBookshelfService
{
    Task<List<BookshelfResponseModel>> GetAllAsync();
    
    Task<List<BookshelfResponseModel>> Get4CategoriesForHomeExploreAsync();
    
    Task<List<BookshelfWithCategoriesResponseModel>> GetAllWithCategoryAsync();
}