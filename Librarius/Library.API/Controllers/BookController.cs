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

    [HttpGet("{bookId:int}")]
    public async Task<IActionResult> GetBookByIdAsync(int bookId)
    {
        var book = await _bookService.GetBookByIdAsync(bookId);

        return Ok(ApiResponse<BookResponseModel>.Success(book));
    }
    
    [HttpGet("{bookId:int}/category")]
    public async Task<IActionResult> GetBookWithCategoryByIdAsync(int bookId)
    {
        var book = await _bookService.GetBookWithCategoryByIdAsync(bookId);

        return Ok(ApiResponse<BookResponseModel>.Success(book));
    }
}