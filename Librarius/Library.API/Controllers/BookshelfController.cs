using Library.API.Models;
using Library.Application.Models.Bookshelf.Response;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BookshelfController : ControllerBase
{
    private readonly IBookshelfService _bookshelfService;

    public BookshelfController(IBookshelfService bookshelfService)
    {
        _bookshelfService = bookshelfService;
    }
    
    // Route: /api/library/bookshelf
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var response = await _bookshelfService.GetAllAsync();

            return Ok(ApiResponse<List<BookshelfResponseModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<List<BookshelfResponseModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/bookshelf/home-explore
    [HttpGet("home-explore")]
    public async Task<IActionResult> Get4CategoriesForHomeExplore()
    {
        try
        {
            var response = await _bookshelfService.Get4CategoriesForHomeExploreAsync();

            return Ok(ApiResponse<List<BookshelfResponseModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<List<BookshelfResponseModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/bookshelf/categories
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllWithCategoryAsync()
    {
        try
        {
            var response = await _bookshelfService.GetAllWithCategoryAsync();
            
            return Ok(ApiResponse<List<BookshelfWithCategoriesResponseModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<List<BookshelfWithCategoriesResponseModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
}