using Library.API.Models;
using Library.Application.Models.Book;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }
    
    // Route: /api/library/book/{bookId}
    [HttpGet("{bookId:int}")]
    public async Task<IActionResult> GetBookWithCategoryByIdAsync(int bookId)
    {
        var response = await _bookService.GetBookWithCategoryByIdAsync(bookId);

        return Ok(ApiResponse<BookResponseModel>.Success(response));
    }
    
    // Route: /api/library/book/{bookId}/read
    // ROute: /api/library/book/read?id=
    [HttpGet("read")]
    public async Task<IActionResult> GetReadingBookByIdAsync([FromQuery] int id)
    {
        var response = await _bookService.GetReadingBookByIdAsync(id);

        return Ok(ApiResponse<BookReadingResponseModel>.Success(response));
    }

    /*
     * Route:
     * Trending Now : /api/library/book/trending?duration=now
     * Trending Week : /api/library/book/trending?duration=week
     */
    [HttpGet("trending")]
    public async Task<IActionResult> GetTrendingBooks([FromQuery] string duration)
    {
        switch (duration)
        {
            case "now":
            {
                // currently trending
                var response = await _bookService.GetTrendingNowBooksAsync();

                return Ok(ApiResponse<IEnumerable<BookTrendingResponseModel>>.Success(response));
            }
            case "week":
            {
                // the top 10 books of the week based on popularity
                var response = await _bookService.GetTrendingWeekBooksAsync();
            
                return Ok(ApiResponse<IEnumerable<BookTrendingResponseModel>>.Success(response));
            }
            default:
                return BadRequest("Invalid parameter.");
        }
    }
}