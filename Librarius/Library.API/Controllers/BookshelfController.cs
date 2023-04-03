using Library.API.Models;
using Library.Application.Models.Bookshelf;
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

    // Get: /Bookshelf
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var response = await _bookshelfService.GetAllAsync();

        return Ok(ApiResponse<List<BookshelfResponseModel>>.Success(response));
    }
}