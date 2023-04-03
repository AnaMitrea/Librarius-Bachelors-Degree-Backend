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

    [HttpGet("{bookId}")]
    public async Task<IActionResult> GetById(int bookId)
    {
        var book = await _bookService.GetByIdAsync(bookId);

        return Ok(ApiResponse<BookResponseModel>.Success(book));
    }
}