using Library.API.Models;
using Library.Application.Models.Category;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // Get: api/library/categories
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var response = await _categoryService.GetAllAsync();

        return Ok(ApiResponse<List<CategoryWithBookshelfResponseModel>>.Success(response));
    }
}