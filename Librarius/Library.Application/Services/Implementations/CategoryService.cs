using AutoMapper;
using Library.Application.Models.Category;
using Library.DataAccess.Repositories;

namespace Library.Application.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<List<CategoryWithBookshelfResponseModel>> GetAllAsync()
    {
        var categories = await this._categoryRepository.GetAllAsync();

        return _mapper.Map<List<CategoryWithBookshelfResponseModel>>(categories);
    }
}