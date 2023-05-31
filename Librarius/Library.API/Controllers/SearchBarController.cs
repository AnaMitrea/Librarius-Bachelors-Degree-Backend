using Library.API.Models;
using Library.Application.Models.Book;
using Library.Application.Models.SearchBar;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[Route("[controller]")]
[ApiController]
public class SearchBarController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IAuthorService _authorService;

    public SearchBarController(IAuthorService authorService, IBookService bookService)
    {
        _authorService = authorService;
        _bookService = bookService;
    }
    
    // Route: /api/library/searchbar
    [HttpPost]
    public async Task<IActionResult> SearchAuthorAndBooksByFilter([FromBody] SearchBarRequestModel requestModel)
    {
        try
        {
            var filteredBooks = await _bookService.SearchBooksByFilterAsync(requestModel);
            var filteredAuthors = await _authorService.SearchAuthorByFilterAsync(requestModel);

            var mergedList = filteredBooks.Cast<object>().Concat(filteredAuthors);

            return Ok(mergedList);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}