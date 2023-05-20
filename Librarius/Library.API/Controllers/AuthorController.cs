using Library.API.Models;
using Library.Application.Models.Book.Author;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }
    
    // Route: /api/library/author/{authorId}
    [HttpGet("{authorId:int}")]
    public async Task<IActionResult> GetAuthorInformationAsync(int authorId)
    {
        try
        {
            var response = await _authorService.GetAuthorInformationByIdAsync(authorId);

            return Ok(ApiResponse<AuthorResponseModel>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<AuthorResponseModel>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
    
    // Route: /api/library/author/materials
    [HttpPost("materials")]
    public async Task<IActionResult> GetBooksAsync(MaterialRequestModel requestModel)
    {
        try
        {
            var response = await _authorService.GetAuthorBooksAsync(requestModel);

            return Ok(ApiResponse<ICollection<MaterialsResponseModel>>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<ICollection<MaterialsResponseModel>>.Fail(new List<ApiValidationError> { new(null, e.Message) }) );
        }
    }
}