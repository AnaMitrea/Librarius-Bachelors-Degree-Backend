using Library.Application.Models.Category;

namespace Library.Application.Services;

public interface ICategoryService
{
    Task<List<CategoryResponseModel>> GetAllAsync();
}