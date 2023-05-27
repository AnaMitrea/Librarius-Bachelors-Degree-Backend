using Library.API.Models;
using Library.Application.Models.LibraryUser;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[Route("user")]
[ApiController]
public class LibraryUserController : ControllerBase
{
    private readonly IUserService _userService;

    public LibraryUserController(IUserService userService)
    {
        _userService = userService;
    }

    // Route: /api/library/user/register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsLibraryUser([FromBody] RegisterUserRequestModel requestModel)
    {
        try
        {
            var response = await _userService.RegisterAsLibraryUser(requestModel);

            return Ok(ApiResponse<bool>.Success(response));
        }
        catch (Exception e)
        {
            return BadRequest(ApiResponse<bool>.Fail(new List<ApiValidationError> { new(null, e.Message) }));

        }
    }
}